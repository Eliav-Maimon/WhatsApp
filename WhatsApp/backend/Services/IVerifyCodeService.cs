using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IVerifyCodeService
    {
        Task SendCodeAsync(string to, string subject, string text);
        int GenerateVerificationCode(int length);
    }
}