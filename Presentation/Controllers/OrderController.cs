using Application.OrderService;
using Core.CoreDtos;
using Core.Extentions;
using Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(TableHubDbContext context) : ControllerBase
    {
        private readonly TableHubDbContext _context = context;

        [HttpGet]
        public async Task<PaggingResultDto<SearchOrderDto>> SearchOrderItem([FromQuery] SearchOrderQuery query)
        {
            var orders = await _context.Orders.AsNoTracking()
                .TakeAvailable()
                .Pagging(query, out var total)
                .ToListAsync();

            var orderIds = orders.Select(x => x.Key)
                .ToList();

            var orderItems = await _context.OrderItems.AsNoTracking()
                .TakeAvailable()
                .Where(x => orderIds.Contains(x.OrderId))
                .ToListAsync();

            var result = orders.Select(x => new SearchOrderDto
            {
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
                Results = result
            };
        }

        [HttpPost]
        public Task<CreateOrderDto> CreateOrder([FromBody] CreateOrderCommand command)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<UpdateOrderDto> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public Task DeleteOrder([FromBody] DeleteOrderCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
