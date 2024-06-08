using System.Net;
using System.Net.Mail;

namespace backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendCodeAsync(string emailAddress, string subject, string text)
        {
            string mail = _configuration["EmailSettings:SenderEmail"];
            string pw = _configuration["EmailSettings:SenderPassword"];

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: emailAddress, subject, text));
        }
    }
}