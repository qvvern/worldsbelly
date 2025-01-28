using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Models;

namespace WorldsBelly.API.Services.Interfaces
{
    public interface IRecipeService
    {
        Task<ActionResult<RecipeView>> GetRecipeAsync(Guid id);
        Task<ActionResult<ICollection<RecipeStepView>>> GetRecipeStepsAsync(Guid id);
        Task<RecipeCollectionResponse<RecipeView>> GetRecipesAsync(RecipeFilterQuery filter);
        Task<RecipeCollectionResponse<RecipeView>> GetRecipesRelatedToUserAsync(RecipeRelatedToUserFilterQuery filter);
        Task<ActionResult<ICollection<RecipeBestServedView>>> GetRecipeBestServedAsync();
        Task<ActionResult<ICollection<RecipeConsumerView>>> GetRecipeConsumerAsync();
        Task<ActionResult<ICollection<RecipeAgeGroupView>>> GetRecipeAgeGroupAsync();
        Task<ActionResult<ICollection<RecipeDifficultyView>>> GetRecipeDifficultyAsync();
        Task<ActionResult<RecipeRatingView>> GetRecipeUserRatingAsync(int recipeId);
        Task<ActionResult<RecipeRatingView>> SubmitRecipeUserRatingAsync(RecipeRatingView rating);
        Task LikeRecipeAsync(int recipeId);
        Task<ActionResult<bool>> GetLikeRecipeAsync(int recipeId);
    }
}
