using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUsersAsync();
        Task<User> CreateUser(Guid id);
        Task<bool> IsUsernameVerifiedAsync(string username);
        Task<User> GetUserByAzureIdAsync(Guid id);
        Task<User> GetUserByIdAsync(int id);
        Task DeleteUserAsync(Guid id);
        Task<User> GetUserByUsernameAsync(string name);
        Task<User> UpdateSignedInUserAsync(User user);
    }
}
