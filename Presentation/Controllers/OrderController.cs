using Application.OrderService;
using Core.CoreDtos;
using Core.Enum;
using Core.Extentions;
using Core.Services.UserService;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;
using System;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(TableHubDbContext context, PayOS payOS, IUserService userService) : ControllerBase
    {
        private readonly TableHubDbContext _context = context;
        private readonly PayOS _payOS = payOS;
        private readonly IUserService _userService = userService;

        [HttpGet]
        public async Task<PaggingResultDto<SearchOrderDto>> SearchOrderItem([FromQuery] SearchOrderQuery query)
        {
            var orders = await _context.Orders.AsNoTracking()
                .TakeByStore(User, query.StoreId)
                .TakeAvailable()
                .Pagging(query, out var total)
                .ToListAsync();

            var orderIds = orders.Select(x => x.Key)
                .ToList();

            var orderItems = await _context.OrderItems.AsNoTracking()
                .TakeAvailable()
                .Where(x => orderIds.Contains(x.OrderId.Value))
                .ToListAsync();

            var result = orders.Select(x => new SearchOrderDto
            {
                OrderId = x.Key,
                CustommerName = x.CustomerId.ToString(),
                TotalPrice = x.TotalPrice,
                Status = x.Status,
                Items = orderItems.Where(item => item.OrderId == x.Key)
                .Select(x => new SearchOrderItemDto
                {
                    ItemName = x.ProductName,
                    Quantity = x.Quantity
                })
                .AsEnumerable()
            });

            return new PaggingResultDto<SearchOrderDto>
            {
                Total = total,
                PageSize = query.PageSize,
                Sequence = query.Sequence,
                Results = result.ToList()
            };
        }

        [HttpPost]
        public async Task<CreateOrderDto> CreateOrder([FromBody] CreateOrderCommand command)
        {
            using var orderTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    CustomerId = _userService.GetUserId(),
                    Status = OrderStatus.Pending,
                    StoreId = command.StoreId,
                    TableId = command.TableId,
                    CheckinCode = GenerateCheckInCode()
                };

                var productIds = command.Items.Select(x => x.ProductId).ToHashSet();

                var products = await _context.Products.AsNoTracking()
                    .TakeAvailable()
                    .Where(x => productIds.Contains(x.Key))
                    .ToListAsync();

                var itemQuery = from item in command.Items
                                join product in products on item.ProductId equals product.Key
                                select new
                                {
                                    ProductId = product.Key,
                                    ProductName = product.Name,
                                    Quantity = item.Quantity,
                                    UnitPrice = product.Price,
                                    Amount = item.Quantity * product.Price
                                };
                var totalAmount = itemQuery.Sum(x => x.Amount);
                order.TotalPrice = totalAmount;
                var orderId = _context.Orders.CreateWithTracking(order);
                await _context.SaveChangesAsync();

                var orderItems = itemQuery.Select(x =>
                {
                    return new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        UnitPrice = x.UnitPrice,
                        Quantity = x.Quantity
                    };
                });

                _context.OrderItems.BulkCreateWithTracking(orderItems);

                await _context.SaveChangesAsync();
                await orderTransaction.CommitAsync();

                if (command.IsOnlinePayment)
                {
                    using var paymentTransaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        var store = await _context.Stores.TakeAvailable()
                            .FirstOrDefaultAsync(x => x.Key == command.StoreId) ?? throw new("Not found Store");

                        var payment = new Payment
                        {
                            Amount = totalAmount,
                            Description = $"{store.Name} - {totalAmount}",
                            Status = PaymentStatus.Processing,
                            OrderId = orderId,
                            StoreId = command.StoreId
                        };

                        _context.Payments.Add(payment);
                        await _context.SaveChangesAsync();

                        var paymentData = await _context.Payments
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Status == PaymentStatus.Processing && x.OrderId == orderId);

                        var items = orderItems.Select(x => new ItemData(x.ProductName, (int) x.Quantity, (int) x.UnitPrice))
                            .ToList();

                        var expiredTime = DateTimeOffset.Now.AddHours(1).ToUnixTimeSeconds();

                        var res = await _payOS.createPaymentLink(new(paymentData.OrderCode,
                                                                     (int) paymentData.Amount,
                                                                     paymentData.Description,
                                                                     items,
                                                                     command.CancelUrl,
                                                                     command.ReturnUrl));
                        await paymentTransaction.CommitAsync();

                        return new CreateOrderDto
                        {
                            OrderId = orderId,
                            PaymentUrl = res.checkoutUrl,
                            RawQrCode = res.qrCode,
                        };
                    }
                    catch
                    {
                        await paymentTransaction.RollbackAsync();
                        throw;
                    }
                }

                return new CreateOrderDto
                {
                    OrderId = orderId
                };
            }
            catch
            {
                await orderTransaction.RollbackAsync();
                throw;
            }
        }

        [HttpPost("checkin")]
        public async Task<CheckinDto> UpdateOrder([FromBody] CheckinCommand command)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _context.Orders.TakeAvailable()
                    .Where(x => x.Status == OrderStatus.Paid)
                    .TakeByStore(User)
                    .FirstOrDefaultAsync(x => x.CheckinCode == command.CheckinCode) ?? throw new("Order not found!");

                order.Status = OrderStatus.Completed;
                _context.Orders.UpdateWithTracking(order);
                await _context.SaveChangesAsync();

                var table = await _context.Tables.TakeAvailable()
                    .FirstOrDefaultAsync(x => x.Key == order.TableId) ?? throw new("Table not found!");

                table.Status = TableStatus.Busy;
                _context.Tables.UpdateWithTracking(table);
                await _context.SaveChangesAsync();

                var orderItem = await _context.OrderItems.TakeAvailable()
                    .Where(x => x.OrderId == order.Key)
                    .Select(x => new CheckinOrderItemDto
                    {
                        Name = x.ProductName,
                        Quantity = x.Quantity
                    })
                    .ToListAsync();

                await transaction.CommitAsync();

                return new()
                {
                    TableName = table.Name,
                    Items = orderItem
                };
            } catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        [HttpDelete]
        public Task DeleteOrder([FromBody] DeleteOrderCommand command)
        {
            throw new NotImplementedException();
        }

        private static string GenerateCheckInCode(int length = 6)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s [random.Next(s.Length)]).ToArray());
        }
    }
}
