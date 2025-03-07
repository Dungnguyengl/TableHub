
using MailerSendNetCore.Emails;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Core.Services.MailService
{
    public class MailerSendService : IMailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromMail;

        public MailerSendService(IConfiguration configuration)
        {
            var host = configuration ["Mail:Host"] ?? string.Empty;
            var port = configuration ["Mail:Port"] ?? string.Empty;
            var username = configuration ["Mail:Username"] ?? string.Empty;
            var pass = configuration ["Mail:Password"] ?? string.Empty;

            _fromMail = configuration ["Mail:MailFrom"] ?? throw new Exception($"Miss Mail From for {nameof(MailerSendService)}");

            _smtpClient = new SmtpClient(host, int.Parse(port))
            {
                Credentials = new NetworkCredential(username, pass),
                EnableSsl = true
            };
        }

        public async Task SendMailAsync(string toMail, string subject, string body)
        {
            var mailMessage = new MailMessage(new MailAddress(_fromMail), new MailAddress(toMail))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
