using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Services.Interfaces
{
    public interface IAzureAdB2CService
    {
        Task<List<AzureAdB2CUser>> GetUsersAsync();
        Task<AzureAdB2CUser> GetUserAsync(string id);
        Task UpdateUserEmail(string id, string newEmail);
        Task<bool> DoesEmailAlreadyExistAsync(string email);
        Task UpdateUserAsync(AzureAdB2CUser user, Guid signedInUserId);
        Task DeleteUser(string id);
        Task ResetUserVerificationCode(string id);
        Task UpdateUserVerificationCode(string id, string oldEmail, string newEmail, string userToken);
        Task UpdateDeleteUserVerificationCode(string id, string userEmail, string userToken);
    }
}
