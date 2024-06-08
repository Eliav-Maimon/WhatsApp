using backend.Models;

namespace backend.Services
{
    public class SMSSerivce : IVerifyCodeService<VerifyCode>
    {
        public Task SendCodeAsync(string to, string subject, string text)
        {
            throw new NotImplementedException();
        }

        public int GenerateVerificationCode(int length)
        {
            throw new NotImplementedException();
        }

        public string GenerateJwtToken(string userId)
        {
            throw new NotImplementedException();
        }

        public Task CreateAndSendVerificationCode(string userId, string email, int code)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAndSendVerificationCode(string verificationId, string email, int code)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAndSendVerificationCode(VerifyCode currentVerify, string email, int newCode)
        {
            throw new NotImplementedException();
        }

        public Task<VerifyCode> GetByUserIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public bool IsTimeExpired(DateTime createdTime)
        {
            throw new NotImplementedException();
        }
    }
}