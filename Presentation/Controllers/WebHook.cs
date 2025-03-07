using Core.Extentions;
using Core.Services.MailService;
using Domain.Constants;
using Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS.Types;

namespace Presentation.Controllers
{
    [Route("/web-hooks")]
    public class WebHook(ILogger<WebHook> logger, TableHubDbContext context, IMailService mailService) : ControllerBase
    {
        private readonly ILogger<WebHook> _logger = logger;
        private readonly TableHubDbContext _context = context;
        private readonly IMailService _mailService = mailService;
        private readonly string _mailTemplate = @"
<!DOCTYPE html>
<html lang=""vi"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Thông tin Check-in</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            color: #333;
        }
        .container {
            max-width: 600px;
            margin: 0 auto;
            background: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        .header {
            text-align: center;
            margin-bottom: 20px;
            background-color: #000; /* Màu nền đen */
            color: #fff; /* Màu chữ trắng */
        }
        .header img {
            max-width: 100px;
        }
        .content {
            text-align: center;
        }
        .code {
            font-size: 24px;
            font-weight: bold;
            color: #FF7F00; /* Màu cam */
            margin: 20px 0;
        }
        .footer {
            text-align: center;
            margin-top: 20px;
            font-size: 12px;
            color: #aaa;
        }

        @media (prefers-color-scheme: dark) {
            body {
                background-color: #333;
                color: #f4f4f4;
            }
            .container {
                background: #444;
                box-shadow: 0 0 10px rgba(255, 255, 255, 0.1);
            }
            .footer {
                color: #bbb;
            }
        }
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <img src=""logo-url"" alt=""Company Logo"">
            <h1>Thông tin Check-in</h1>
        </div>
        <div class=""content"">
            <p>Xin chào,</p>
            <p>Đây là mã check-in của bạn:</p>
            <p class=""code"">{{CheckinCode}}</p> <!-- Đoạn mã 6 ký tự in hoa gồm chữ và số -->
            <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
        </div>
        <div class=""footer"">
            <p>&copy; 2025 Công ty của bạn. Mọi quyền được bảo lưu.</p>
        </div>
    </div>
</body>
</html>
";

        [HttpPost("payment-info")]
        public async Task<IActionResult> PaymentInfo([FromBody] WebhookType input)
        {
            if (input.success)
            {
                var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var payment = await _context.Payments.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.OrderCode == input.data.orderCode) ?? throw new("Payment not found!");

                    payment.Status = Core.Enum.PaymentStatus.Paid;
                    payment.PaidAt = DateTime.Parse(input.data.transactionDateTime);
                    payment.Reference = input.data.reference;
                    _context.Payments.Update(payment);
                    await _context.SaveChangesAsync();

                    var order = await _context.Orders.TakeAvailable()
                        .FirstOrDefaultAsync(x => x.Key == payment.OrderId) ?? throw new("Order not found!");

                    order.Status = Core.Enum.OrderStatus.Paid;
                    _context.Orders.UpdateWithTracking(order);
                    await _context.SaveChangesAsync();

                    var table = _context.Tables.TakeAvailable()
                        .FirstOrDefault(x => x.Key == order.TableId) ?? throw new("Table not found!");

                    table.Status = Core.Enum.TableStatus.Ordered;
                    _context.Tables.UpdateWithTracking(table);
                    await _context.SaveChangesAsync();

                    var customer = await _context.Users.AsNoTracking()
                        .Where(x => x.Role == Roles.GUEST)
                        .FirstOrDefaultAsync(x => x.Id == order.CustomerId.ToString()) ?? throw new("Customer not found!");

                    var content = _mailTemplate.Replace("{{CheckinCode}}", order.CheckinCode);
                    var mail = customer.Email ?? throw new("Customer's mail not found!");
                    await _mailService.SendMailAsync(mail, "[TABLE_HUB] Thông Tin Check-In Của Bạn", content);

                    await transaction.CommitAsync();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Exeption when handle payment Success");
                    await transaction.RollbackAsync();
                    return StatusCode(500);
                }
            }
            return Ok(new { Success = true });
        }
    }
}
