namespace backend.Services
{
    public class SMSSerivce : IVerifyCodeService
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
    }
}