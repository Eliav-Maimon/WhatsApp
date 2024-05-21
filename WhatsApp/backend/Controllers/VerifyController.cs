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

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string email)
        {
            int.TryParse(_configuration["EmailSettings:VerifyCodeLength"], out int number);

            int code = _verifyService.GenerateVerificationCode(number > 0 ? number : 4);

            var currentUser = await _mongoDatabase.GetUserByEmailAsync(email);

            var currentVerify = await _verifyRepository.GetByUserIdAsync(currentUser.Id);

            if (currentVerify == null)
            {
                var newVerifyCode = new VerifyCode { UserId = currentUser.Id, CodeNumber = code };
                await _verifyRepository.PostAsync(newVerifyCode);
                await _verifyService.SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {code}");
            }
            else if (currentVerify.UserId == currentUser.Id)
            {
                var currentTime = DateTime.Now.Date;
                TimeSpan difference = currentTime - currentVerify.CreatedTime;

                if (difference.TotalMinutes > 5)
                {
                    // maybe replace to put insted
                    await _verifyRepository.DeleteAsync(currentVerify.Id);
                    var newVerifyCode = new VerifyCode { UserId = currentUser.Id, CodeNumber = code };
                    await _verifyRepository.PostAsync(newVerifyCode);
                    await _verifyService.SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {code}");
                }
                else
                {
                    await _verifyService.SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {currentVerify.CodeNumber}");
                }
            }
            else
            {
                var newVerifyCode = new VerifyCode { UserId = currentUser.Id, CodeNumber = code };
                await _verifyRepository.PostAsync(newVerifyCode);
                await _verifyService.SendCodeAsync(email, "WhatsApp Verify Code", $"Verify Code: {code}");
            }


            return Ok();
        }
    }
}