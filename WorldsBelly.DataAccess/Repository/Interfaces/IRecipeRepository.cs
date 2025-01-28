using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Models;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
    public interface IRecipeRepository
    {
        IQueryable<Recipe> GetRecipes(RecipeFilterQuery filter);
        IQueryable<Recipe> GetRecipesRelatedToUser(RecipeRelatedToUserFilterQuery filter);
        Task<Recipe> GetRecipeTranslationAsync(Guid recipeId);
        Task<ICollection<RecipeStep>> GetRecipeStepsTranslationAsync(Guid recipeId);
        Task<Recipe> CreateRecipeDraftAsync(Recipe recipe);
        Task<ICollection<RecipeBestServed>> GetRecipeBestServedAsync();
        Task<ICollection<RecipeConsumer>> GetRecipeConsumerAsync();
        Task<ICollection<RecipeAgeGroup>> GetRecipeAgeGroupAsync();
        Task<ICollection<RecipeDifficulty>> GetRecipeDifficultyAsync();
        Task<RecipeRating> GetRecipeUserRatingAsync(int recipeId);
        Task CreateRecipeUserRatingAsync(RecipeRating rating);
        Task RemoveRecipeUserRatingAsync(RecipeRating rating);
        Task UpdateRecipeUserRatingAsync(RecipeRating rating);
        Task LikeRecipeAsync(int recipeId);
        Task<bool> GetLikeRecipeAsync(int recipeId);

    }
}
