namespace Core.Services.MailService
{
    public interface IMailService
    {
        public Task SendMailAsync(string toMail, string subject, string body);
    }
}
