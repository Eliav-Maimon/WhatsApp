using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Services
{
    // public interface IVerifyCodeService<T> where T : VerifyCode
    // {
    //     Task SendCodeAsync(string to, string subject, string text);
    //     Task CreateAndSendVerificationCode(string userId, string email, int code);
    //     Task UpdateAndSendVerificationCode(string verificationId, string email, int code);
    //     Task HandleExistingVerification(VerifyCode currentVerify, string email, int newCode);
    //     Task<T> GetByUserIdAsync(string id);
    //     bool IsTimeExpired(DateTime createdTime);
    //     int GenerateVerificationCode(int length);
    //     string GenerateJwtToken(string userId);
    // }




    public interface IVerifyCodeService<T> where T : VerifyCode
    {
        Task<T> GetByUserIdAsync(string id);
        Task CreateAndSendVerificationCode(string userId, string email, int code);
        Task UpdateAndSendVerificationCode(T currentVerify, string email, int newCode);
        bool IsTimeExpired(DateTime createdTime);
        int GenerateVerificationCode(int length);
    }

    public interface IEmailService
    {
        Task SendCodeAsync(string to, string subject, string text);
    }

    public interface ITokenService
    {
        string GenerateJwtToken(string userId);
    }

}