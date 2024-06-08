using backend.Models;
using backend.Services;
using WhatsApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("verify")]
    public class VerifyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IVerifyCodeService<VerifyCode> _verifyService;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository<User> _database;

        public VerifyController(IConfiguration configuration, IVerifyCodeService<VerifyCode> verifyService, ITokenService tokenService, IUserRepository<User> database)
        {
            _configuration = configuration;
            _verifyService = verifyService;
            _tokenService = tokenService;
            _database = database;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string email, int code)
        {
            var user = await _database.GetUserByEmailAsync(email);
            if (user == null) return BadRequest("User not found.");

            var currentVerify = await _verifyService.GetByUserIdAsync(user.Id);
            if (currentVerify == null || currentVerify.CodeNumber != code || _verifyService.IsTimeExpired(currentVerify.CreatedTime))
            {
                return BadRequest("Invalid Code");
            }

            return Ok(_tokenService.GenerateJwtToken(user.Id));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!int.TryParse(_configuration["EmailSettings:VerifyCodeLength"], out int codeLength))
            {
                codeLength = 4;
            }

            int code = _verifyService.GenerateVerificationCode(codeLength);

            var user = await _database.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var currentVerify = await _verifyService.GetByUserIdAsync(user.Id);
            if (currentVerify == null)
            {
                await _verifyService.CreateAndSendVerificationCode(user.Id, request.Email, code);
            }
            else
            {
                await _verifyService.UpdateAndSendVerificationCode(currentVerify, request.Email, code);
            }

            return Ok();
        }
    }
}