using Core.Services.MailService;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController(IMailService mailService) : ControllerBase
    {
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
            <p class=""code"">ABC123</p> <!-- Đoạn mã 6 ký tự in hoa gồm chữ và số -->
            <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
        </div>
        <div class=""footer"">
            <p>&copy; 2025 Công ty của bạn. Mọi quyền được bảo lưu.</p>
        </div>
    </div>
</body>
</html>
";


        [HttpGet]
        public async Task<ActionResult> SendMail()
        {
            await _mailService.SendMailAsync("dungntqe170072@fpt.edu.vn", "Test", _mailTemplate);

            return Ok();
        }
    }
}
