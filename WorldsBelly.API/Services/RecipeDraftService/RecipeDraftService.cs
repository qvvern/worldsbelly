using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.API.Utilities.Mappers;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;
using WorldsBelly.Domain.Utils.Helpers;

namespace WorldsBelly.API.Services
{
    public class RecipeDraftService : IRecipeDraftService
    {
        private IRecipeDraftRepository _recipeDraftRepository;
        private IRecipeRepository _recipeRepository;
        private ITagRepository _tagRepository;
        private IUserRepository _userRepository;
        private readonly IHeaderService _headerService;

        public RecipeDraftService(IRecipeDraftRepository recipeDraftRepository, IRecipeRepository recipeRepository, ITagRepository tagRepository, IUserRepository userRepository, IHeaderService headerService)
        {
            _recipeDraftRepository = recipeDraftRepository;
            _recipeRepository = recipeRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _headerService = headerService;
        }
        public async Task<ActionResult<ICollection<RecipeView>>> GetRecipeDraftsAsync()
        {
            var response = await _recipeDraftRepository.GetRecipeDraftTranslationsAsync();
            return response.Select(ResponseMapper.Map).ToList();
        }

        public async Task<ActionResult<RecipeView>> GetRecipeDraftAsync(Guid id)
        {
            var response = await _recipeDraftRepository.GetRecipeDraftTranslationAsync(id);
            return ResponseMapper.Map(response);

        }

        public async Task<RecipeView> UpdateRecipeDraftAsync(Guid id, RecipeView recipeDraft)
        {
            if(id != recipeDraft.Id)
            {
                throw new InvalidOperationException("Id is not same as recipe id");
            }
            Recipe entity = RequestMapper.Map(recipeDraft);
            await _recipeDraftRepository.UpdateRecipeDraftAsync(entity, recipeDraft.LanguageId);
            var response = await _recipeDraftRepository.GetRecipeDraftTranslationAsync(id);
            return ResponseMapper.Map(response);
        }

        public async Task PublishRecipeDraftAsync(Guid id)
        {
            await _recipeDraftRepository.PublishRecipeDraftAsync(id);
        }

        public async Task<ActionResult> DeleteRecipeDraftTranslationAsync(Guid id)
        {
            Guid signedInUserId = _headerService.GetUserId();
            var recipeDraft = await _recipeDraftRepository.GetRecipeDraftTranslationAsync(id);
            if(signedInUserId == recipeDraft.CreatedByUser.ADObjectId && id == recipeDraft.Translations.First().Id)
            {
                return await _recipeDraftRepository.DeleteRecipeAsync(recipeDraft.Id);
            }
            else
            {
                throw new Exception("User does not match");
            }

        }

        public async Task<ActionResult<RecipeView>> CreateRecipeDraftTranslationAsync(CreateRecipeDraftView recipeDraft)
        {
            if (recipeDraft == null)
            {
                throw new Exception("Recipe was not defined");
            }

            int languageId = _headerService.GetLanguageId().GetValueOrDefault(); 
            Guid signedInUserId = _headerService.GetUserId();
            User user = await _userRepository.GetUserByAzureIdAsync(signedInUserId);

            // Recipe
            Recipe recipe = new Recipe(user.Id, languageId);

            // Recipe Translation
            var translations = recipe.Translations.ToList();
            translations.Add(new RecipeTranslation(user.Id, languageId));
            recipe.Translations = translations;

            // Define consumer
            recipe.ConsumerId = 1;
            //ICollection<RecipeConsumer> consumers = await _recipeRepository.GetRecipeConsumerAsync();
            //foreach(RecipeConsumer consumer in consumers)
            //{
            //    if(recipeDraft.TagIds.Any(id => id == consumer.TagId))
            //    {
            //        recipe.ConsumerId = consumer.Id;
            //        recipeDraft.TagIds.Remove(consumer.TagId);
            //    }
            //}

            // Define age group
            recipe.AgeGroupId = 3;
            //ICollection <RecipeAgeGroup> ageGroups = await _recipeRepository.GetRecipeAgeGroupAsync();
            //foreach (RecipeAgeGroup ageGroup in ageGroups)
            //{
            //    if (recipeDraft.TagIds.Any(id => id == ageGroup.TagId))
            //    {
            //        recipe.AgeGroupId = ageGroup.Id;
            //        recipeDraft.TagIds.Remove(ageGroup.TagId);
            //    }
            //}

            // Recipe Tags
            ICollection<Tag> tags = await _tagRepository.GetTagsByIdsAsync(recipeDraft.TagIds);
            recipe.Tags = tags;

            // Recipe Steps
            var steps = recipe.Steps.ToList();
            steps.Add(new RecipeStep());
            var stepsTranslations = steps.First().Translations.ToList();
            stepsTranslations.Add(new RecipeStepTranslation(languageId));
            steps.First().Translations = stepsTranslations;
            recipe.Steps = steps;

            // Recipe RecipeIngredientList
            var ingredientLists = recipe.IngredientLists.ToList();
            ingredientLists.Add(new RecipeIngredientList());
            var ingredientListsTranslations = ingredientLists.First().Translations.ToList();
            ingredientListsTranslations.Add(new RecipeIngredientListTranslation(languageId));
            ingredientLists.First().Translations = ingredientListsTranslations;
            recipe.IngredientLists = ingredientLists;

            // CREATE
            var response = await _recipeDraftRepository.CreateRecipeDraftAsync(recipe);
            return ResponseMapper.Map(response);
        }
    }
}
