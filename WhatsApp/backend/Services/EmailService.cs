using System.Net;
using System.Net.Mail;

namespace backend.Services
{
    public class EmailService : IVerifyCodeService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            this._configuration = configuration;
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

        public int GenerateVerificationCode(int length)
        {
            Random random = new Random();
            string temp = "";
            for (int i = 0; i < length; i++)
            {
                temp += random.Next(1, 10).ToString();
            }

            int.TryParse(temp, out int code);
            return code;
        }
    }
}