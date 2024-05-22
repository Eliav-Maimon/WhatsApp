using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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

        public string GenerateJwtToken(string userId)
        {
            var jwtConfig = _configuration.GetSection("JwtSettings");
            var key = jwtConfig["Key"];
            var expirationTimeInMinutes = int.TryParse(jwtConfig["ExpirationTimeInMinutes"], out int expTime) ? expTime : 1440;
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var tokenLifetime = TimeSpan.FromMinutes(expirationTimeInMinutes);

            // claims is about what the token will contain/create
            var claims = new List<Claim>
        {
            // jwt id = jti
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, userId),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(tokenLifetime),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}