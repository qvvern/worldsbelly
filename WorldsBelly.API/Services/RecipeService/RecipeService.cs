using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.API.Utilities.Mappers;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Migrations;
using WorldsBelly.DataAccess.Models;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;
using WorldsBelly.Domain.Utils.Helpers;

namespace WorldsBelly.API.Services
{
    public class RecipeService : IRecipeService
    {
        private IRecipeRepository _recipeRepository;
        private readonly IHeaderService _headerService;
        private IUserRepository _userRepository;
        public RecipeService(IRecipeRepository recipeRepository, IHeaderService headerService, IUserRepository userRepository)
        {
            _recipeRepository = recipeRepository;
            _headerService = headerService;
            _userRepository = userRepository;
        }

        public async Task<RecipeCollectionResponse<RecipeView>> GetRecipesAsync(RecipeFilterQuery filter)
        {
            var response = _recipeRepository.GetRecipes(filter);
            var mappedResponse = new RecipeCollectionResponse<RecipeView>()
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                ExactMatch = true,
                Recipes = response.Select(ResponseMapper.Map).ToList()
            };
            return mappedResponse;
        }

        public async Task<RecipeCollectionResponse<RecipeView>> GetRecipesRelatedToUserAsync(RecipeRelatedToUserFilterQuery filter)
        {
            var response = _recipeRepository.GetRecipesRelatedToUser(filter);
            var mappedResponse = new RecipeCollectionResponse<RecipeView>()
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                ExactMatch = true,
                Recipes = response.Select(ResponseMapper.Map).ToList()
            };
            return mappedResponse;
        }
        public async Task<ActionResult<RecipeView>> GetRecipeAsync(Guid id)
        {
            var response = await _recipeRepository.GetRecipeTranslationAsync(id);
            return ResponseMapper.Map(response);
        }
        public async Task<ActionResult<ICollection<RecipeStepView>>> GetRecipeStepsAsync(Guid id)
        {
            var response = await _recipeRepository.GetRecipeStepsTranslationAsync(id);
            return response.Select(ResponseMapper.Map).ToList();
        }
        public async Task<ActionResult<ICollection<RecipeBestServedView>>> GetRecipeBestServedAsync()
        {
            var response = await _recipeRepository.GetRecipeBestServedAsync();
            return response.Select(ResponseMapper.Map).ToList();
        }

        public async Task<ActionResult<ICollection<RecipeConsumerView>>> GetRecipeConsumerAsync()
        {
            var response = await _recipeRepository.GetRecipeConsumerAsync();
            return response.Select(ResponseMapper.Map).ToList();
        }

        public async Task<ActionResult<ICollection<RecipeAgeGroupView>>> GetRecipeAgeGroupAsync()
        {
            var response = await _recipeRepository.GetRecipeAgeGroupAsync();
            return response.Select(ResponseMapper.Map).ToList();
        }

        public async Task<ActionResult<ICollection<RecipeDifficultyView>>> GetRecipeDifficultyAsync()
        {
            var response = await _recipeRepository.GetRecipeDifficultyAsync();
            return response.Select(ResponseMapper.Map).ToList();
        }

        public async Task<ActionResult<RecipeRatingView>> GetRecipeUserRatingAsync(int recipeId)
        {
            var response = await _recipeRepository.GetRecipeUserRatingAsync(recipeId);
            return ResponseMapper.Map(response);
        }
        public async Task<ActionResult<RecipeRatingView>> SubmitRecipeUserRatingAsync(RecipeRatingView rating)
        {
            if (rating.RecipeId <= 0) throw new Exception("Could not find recipe");
            var response = await _recipeRepository.GetRecipeUserRatingAsync(rating.RecipeId);
            if (response == null)
            {
                if (rating.Rating == null || rating.Rating < 0 || rating.Rating > 10)
                    throw new Exception("Could not rate recipe");

                Guid signedInUserId = _headerService.GetUserId();
                User user = await _userRepository.GetUserByAzureIdAsync(signedInUserId);

                RecipeRating recipeRating = new RecipeRating();
                recipeRating.Rating = rating.Rating.GetValueOrDefault();
                recipeRating.RecipeId = rating.RecipeId;
                recipeRating.UserId = user.Id;

                await _recipeRepository.CreateRecipeUserRatingAsync(recipeRating);
            }
            else if (rating.Rating == null)
            {
                await _recipeRepository.RemoveRecipeUserRatingAsync(response);
            }
            else
            {
                if (rating.Rating == null || rating.Rating < 0 || rating.Rating > 10)
                    throw new Exception("Could not rate recipe");

                response.Rating = rating.Rating.GetValueOrDefault();
                await _recipeRepository.UpdateRecipeUserRatingAsync(response);
            }
            return rating;
        }

        public async Task LikeRecipeAsync(int recipeId)
        {
            await _recipeRepository.LikeRecipeAsync(recipeId);
        }
        public async Task<ActionResult<bool>> GetLikeRecipeAsync(int recipeId)
        {
            return await _recipeRepository.GetLikeRecipeAsync(recipeId);
        }
    }
}
