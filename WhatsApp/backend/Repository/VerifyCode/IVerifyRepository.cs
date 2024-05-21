namespace backend.Repository.VerifyCodeRepository
{
    public interface IVerifyRepository<VerifyCode>
    {
        Task<IReadOnlyCollection<VerifyCode>> GetAllAsync();
        Task<VerifyCode> GetByUserIdAsync(string id);
        Task<VerifyCode> PostAsync(VerifyCode verifyCode);
        Task<VerifyCode> PutAsync(string id, int code);
        Task<bool> DeleteAsync(string id);
    }
}