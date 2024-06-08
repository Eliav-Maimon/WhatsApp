using backend.Models;
using backend.Repository.VerifyCodeRepository;

namespace backend.Services
{
    public class VerifyCodeService : IVerifyCodeService<VerifyCode>
    {
        private readonly IConfiguration _configuration;
        private readonly IVerifyRepository<VerifyCode> _verifyRepository;
        private readonly IEmailService _emailService;

        public VerifyCodeService(IConfiguration configuration, IVerifyRepository<VerifyCode> verifyRepository, IEmailService emailService)
        {
            _configuration = configuration;
            _verifyRepository = verifyRepository;
            _emailService = emailService;
        }

        public async Task<VerifyCode> GetByUserIdAsync(string id)
        {
            return await _verifyRepository.GetByUserIdAsync(id);
        }

        public async Task CreateAndSendVerificationCode(string userId, string email, int code)
        {
            var newVerifyCode = new VerifyCode
            {
                UserId = userId,
                CodeNumber = code,
                CreatedTime = DateTime.UtcNow
            };

            await _verifyRepository.PostAsync(newVerifyCode);
            await _emailService.SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {code}");
        }

        public async Task UpdateAndSendVerificationCode(VerifyCode currentVerify, string email, int newCode)
        {
            if (IsTimeExpired(currentVerify.CreatedTime))
            {
                await _verifyRepository.PutAsync(currentVerify.Id, newCode);
                await _emailService.SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {newCode}");
            }
            else
            {
                await _emailService.SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {currentVerify.CodeNumber}");
            }
        }


        // public async Task UpdateAndSendVerificationCode(string verificationId, string email, int code)
        // {
        //     await _verifyRepository.PutAsync(verificationId, code);
        //     await SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {code}");
        // }


        // public async Task HandleExistingVerification(VerifyCode currentVerify, string email, int newCode)
        // {
        //     // if time past it return true
        //     if (IsTimeExpired(currentVerify.CreatedTime))
        //     {
        //         // await _verifyRepository.DeleteAsync(currentVerify.Id);
        //         // await CreateAndSendVerificationCode(userId, email, newCode);

        //         await UpdateAndSendVerificationCode(currentVerify.Id, email, newCode);
        //     }

        //     // send code to email
        //     await SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {currentVerify.CodeNumber}");
        // }



        // public async Task CreateAndSendVerificationCode(string userId, string email, int code)
        // {
        //     var newVerifyCode = new VerifyCode
        //     {
        //         UserId = userId,
        //         CodeNumber = code,
        //         CreatedTime = DateTime.UtcNow
        //     };

        //     await _verifyRepository.PostAsync(newVerifyCode);
        //     await SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {code}");
        // }

        public bool IsTimeExpired(DateTime createdTime)
        {
            var expirationTimeInMinutes = 5; // Default expiration time
            if (int.TryParse(_configuration["EmailSettings:ExpirationTimeInMinutes"], out int expTime))
            {
                expirationTimeInMinutes = expTime;
            }

            var currentTime = DateTime.UtcNow;
            return (currentTime - createdTime).TotalMinutes > expirationTimeInMinutes;
        }


        // public bool IsTimeExpired(DateTime createdTime)
        // {
        //     if (!int.TryParse(_configuration["EmailSettings:ExpirationTimeInMinutes"], out int expirationTimeInMinutes))
        //     {
        //         expirationTimeInMinutes = 5;
        //     }

        //     // Get the current date and time in UTC
        //     var currentTime = DateTime.UtcNow;

        //     // Ensure createdTime is also treated as UTC
        //     if (createdTime.Kind == DateTimeKind.Unspecified)
        //     {
        //         createdTime = DateTime.SpecifyKind(createdTime, DateTimeKind.Utc);
        //     }
        //     else if (createdTime.Kind == DateTimeKind.Local)
        //     {
        //         createdTime = createdTime.ToUniversalTime();
        //     }

        //     // Calculate the time difference
        //     TimeSpan difference = currentTime - createdTime;

        //     // Check if the total minutes difference is greater than the expiration time
        //     return difference.TotalMinutes > expirationTimeInMinutes;
        // }


        public int GenerateVerificationCode(int length)
        {
            var random = new Random();
            var code = string.Concat(Enumerable.Range(0, length).Select(_ => random.Next(10).ToString()));
            return int.TryParse(code, out int result) ? result : 0;
        }

        // public int GenerateVerificationCode(int length)
        // {
        //     Random random = new Random();
        //     string temp = "";
        //     for (int i = 0; i < length; i++)
        //     {
        //         temp += random.Next(1, 10).ToString();
        //     }

        //     int.TryParse(temp, out int code);
        //     return code;
        // }
    }
}