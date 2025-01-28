using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Services.Interfaces
{
    public interface IAzureGraphEmailService
    {
        Task SendUserVerificationCodeEmail(string oldEmail, string newEmail, string verificationCode);
        Task SendDeleteUserVerificationCodeEmail(string email, string verificationCode);
    }
}
