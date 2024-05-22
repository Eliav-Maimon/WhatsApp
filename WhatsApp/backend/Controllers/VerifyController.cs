using backend.Models;
using backend.Repository.VerifyCodeRepository;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using WhatsApp.Repository;

namespace backend.Controllers
{
    [ApiController]
    [Route("verify")]
    public class VerifyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IVerifyCodeService _verifyService;
        private readonly IVerifyRepository<VerifyCode> _verifyRepository;
        private readonly IUserRepository<User> _mongoDatabase;

        public VerifyController(IConfiguration configuration, IVerifyCodeService verifyService, IVerifyRepository<VerifyCode> verifyRepository, IUserRepository<User> mongoDatabase)
        {
            this._configuration = configuration;
            this._verifyService = verifyService;
            this._verifyRepository = verifyRepository;
            this._mongoDatabase = mongoDatabase;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string email, int code)
        {
            var user = await _mongoDatabase.GetUserByEmailAsync(email);
            var currentVerify = await _verifyRepository.GetByUserIdAsync(user.Id);

            if (currentVerify.CodeNumber == code && !IsTimeExpired(currentVerify.CreatedTime))
            {
                return Ok(_verifyService.GenerateJwtToken(user.Id));
            }
            else
            {
                return BadRequest("Invalid Code");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email cannot be empty.");
            }

            if (!int.TryParse(_configuration["EmailSettings:VerifyCodeLength"], out int codeLength))
            {
                codeLength = 4;
            }

            int code = _verifyService.GenerateVerificationCode(codeLength);

            var currentUser = await _mongoDatabase.GetUserByEmailAsync(email);

            if (currentUser == null)
            {
                return BadRequest("User not found.");
            }

            var currentVerify = await _verifyRepository.GetByUserIdAsync(currentUser.Id);
            if (currentVerify == null)
            {
                await CreateAndSendVerificationCode(currentUser.Id, email, code);
            }
            else
            {
                await HandleExistingVerification(currentVerify, email, code);
            }

            return Ok();
        }

        private async Task CreateAndSendVerificationCode(string userId, string email, int code)
        {
            var newVerifyCode = new VerifyCode
            {
                UserId = userId,
                CodeNumber = code,
                CreatedTime = DateTime.UtcNow
            };

            await _verifyRepository.PostAsync(newVerifyCode);
            await _verifyService.SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {code}");
        }

        private async Task UpdateAndSendVerificationCode(string verificationId, string email, int code)
        {
            await _verifyRepository.PutAsync(verificationId, code);
            await _verifyService.SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {code}");
        }

        private async Task HandleExistingVerification(VerifyCode currentVerify, string email, int newCode)
        {
            // if time past it return true
            if (IsTimeExpired(currentVerify.CreatedTime))
            {
                // await _verifyRepository.DeleteAsync(currentVerify.Id);
                // await CreateAndSendVerificationCode(userId, email, newCode);

                await UpdateAndSendVerificationCode(currentVerify.Id, email, newCode);
            }

            // send code to email
            await _verifyService.SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {currentVerify.CodeNumber}");
        }

        private bool IsTimeExpired(DateTime createdTime)
        {
            if (!int.TryParse(_configuration["EmailSettings:ExpirationTimeInMinutes"], out int expirationTimeInMinutes))
            {
                expirationTimeInMinutes = 5;
            }

            // Get the current date and time in UTC
            var currentTime = DateTime.UtcNow;

            // Ensure createdTime is also treated as UTC
            if (createdTime.Kind == DateTimeKind.Unspecified)
            {
                createdTime = DateTime.SpecifyKind(createdTime, DateTimeKind.Utc);
            }
            else if (createdTime.Kind == DateTimeKind.Local)
            {
                createdTime = createdTime.ToUniversalTime();
            }

            // Calculate the time difference
            TimeSpan difference = currentTime - createdTime;

            // Check if the total minutes difference is greater than the expiration time
            return difference.TotalMinutes > expirationTimeInMinutes;
        }

    }
}