using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;

namespace WorldsBelly.DataAccess.Repository
{
	public class UserRepository : IUserRepository
    {
        private readonly IHeaderService _headerService;
        private readonly AppDbContext _dbContext;
        private readonly IAzureAdB2CService _azureAdB2CService;
        private IImageRepository _imageRepository;

        public UserRepository(AppDbContext dbContext, IAzureAdB2CService azureAdB2CService, IHeaderService headerService, IImageRepository imageRepository)
        {
            _dbContext = dbContext;
            _azureAdB2CService = azureAdB2CService;
            _headerService = headerService;
            _imageRepository = imageRepository;
        }
        public async Task<bool> IsUsernameVerifiedAsync(string username)
        {
            Regex regex = new Regex(@"[^\p{L}\p{M}\p{N}]+");
            if (username.IsNull() || username.Length < 3 || username.Length > 50 || regex.IsMatch(username))
            {
                return false;
            }

            User user = await GetUserByUsernameAsync(username);
            return user == null;
        }
        private async Task<string> GenerateUsername(string firstName, string lastName)
        {
            var combined = firstName.Trim() + lastName.Trim(); 
            var username = Regex.Replace(combined, @"[^\p{L}\p{M}\p{N}]+", "");
            if(username.Length < 3)
            {
                username = username + "123";
            }
            if (username.Length > 50)
            {
                username = new string(username.Take(49).ToArray());
            }

            int maxNumber = 1005;

            for (int i = 1; i <= maxNumber; i++)
            {
                if (await IsUsernameVerifiedAsync(username))
                {
                    break;
                }
                else
                {
                    var newUsername = username;
                    if (i < 1000)
                    {
                       newUsername = username + i;
                    }
                    else
                    {
                        var code = GenerateSecurityCode();
                        newUsername = Regex.Replace(username + code, @"[^\p{L}\p{M}\p{N}]+", "");
                    }
                    if (await IsUsernameVerifiedAsync(newUsername))
                    {
                        username = newUsername;
                        break;
                    }
                }
            }

            return username;
        }
        private string GenerateSecurityCode()
        {
            var randomNumber = new byte[4];
            string refreshToken = "";

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
            }
            return refreshToken;
        }
        public async Task<User> CreateUser(Guid id)
        {
            var azureUser = await _azureAdB2CService.GetUserAsync(id.ToString());
            var firstName = azureUser.GivenName;
            var lastName = azureUser.Surname;

            User newUser = new User()
            {
                ADObjectId = id,
                Username = await GenerateUsername(firstName, lastName)
            };
            newUser.CreatedAt = DateTime.UtcNow;
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return newUser;
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> GetUserByAzureIdAsync(Guid id)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.ADObjectId == id);
        }
        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.ADObjectId == id);

            if(user != null)
            {
                List<RecipeTranslation> recipeTranslations = await _dbContext.RecipeTranslations.Where(i => i.CreatedByUserId == user.Id).ToListAsync();
                List<int> toDelete = new List<int>();
                if (recipeTranslations != null && recipeTranslations.Count > 0)
                {
                    foreach (RecipeTranslation recipeTranslation in recipeTranslations)
                    {
                        if (recipeTranslation.IsApproved)
                        {
                            recipeTranslation.CreatedByUserId = 1;
                        }
                        else
                        {
                            toDelete.Add(recipeTranslation.RecipeId);
                        }
                    }
                }

                List<Recipe> recipes = await _dbContext.Recipes.Where(i => i.CreatedByUserId == user.Id).ToListAsync();
                if(recipes != null && recipes.Count > 0)
                {
                    foreach(Recipe recipe in recipes)
                    {
                        if(toDelete.Contains(recipe.Id))
                        {
                            _dbContext.Remove(recipe);
                        }
                        else
                        {
                            recipe.CreatedByUserId = 1;
                        }
                    }
                }
                _dbContext.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Cannot delete user, because user could not be found.");
            }
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
        }

		public async Task<ICollection<User>> GetUsersAsync()
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        }
        public async Task<User> UpdateSignedInUserAsync(User updatedUser)
        {
            Guid signedInUserId = _headerService.GetUserId();
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.ADObjectId == signedInUserId);
            if (user == null || updatedUser.ADObjectId != signedInUserId)
            {
                throw new Exception("Could not find user");
            }

            // USERNAME
            if (!String.IsNullOrEmpty(updatedUser.Username) && user.Username != updatedUser.Username)
            {
                bool isUsernameVerified = await IsUsernameVerifiedAsync(updatedUser.Username);
                if (isUsernameVerified)
                {
                    user.Username = updatedUser.Username.Trim();
                }
            }

            // profile image
            if (!String.IsNullOrEmpty(updatedUser.Image) && updatedUser.Image.StartsWith("data"))
            {
                user.Image = await _imageRepository.StoreImageAsync(updatedUser.Image, $"{user.Id}", name: "main", maxBoth: 300, blobContainerName: "user-images");
            }
            else if (String.IsNullOrEmpty(updatedUser.Image) && !String.IsNullOrEmpty(user.Image))
            {
                await _imageRepository.RemoveImageFolderAsync($"{user.Id}", blobContainerName: "user-images");
                user.Image = null;
            }

            user.Summary = updatedUser.Summary;
            user.References = updatedUser.References;

            user.UpdatedAt = DateTime.UtcNow;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
