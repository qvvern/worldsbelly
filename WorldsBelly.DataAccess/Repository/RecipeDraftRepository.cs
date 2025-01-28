using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static MoreLinq.Extensions.LagExtension;
using static MoreLinq.Extensions.LeadExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;
using PuppeteerSharp;
using System.Reflection;

namespace WorldsBelly.DataAccess.Repository
{
    public class RecipeDraftRepository : IRecipeDraftRepository
    {
        private readonly AppDbContext _dbContext;
        private IImageRepository _imageRepository;
        private IUserRepository _userRepository;
        private readonly IHeaderService _headerService;

        public RecipeDraftRepository(AppDbContext dbContext, IImageRepository imageRepository, IUserRepository userRepository, IHeaderService headerService)
        {
            _dbContext = dbContext;
            _imageRepository = imageRepository;
            _userRepository = userRepository;
            _headerService = headerService;
        }

        public async Task<Recipe> CreateRecipeDraftAsync(Recipe entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            _dbContext.Recipes.Add(entity);
            await _dbContext.SaveChangesAsync();

            return await GetRecipeDraftTranslationAsync(entity.Translations.First().Id);
        }

        public async Task<Recipe> GetRecipeDraftTranslationAsync(Guid id)
        {
            Guid signedInUserId = _headerService.GetUserId();
            RecipeTranslation recipeTranslation = _dbContext.RecipeTranslations.Single(_ => _.Id == id && (signedInUserId == _.CreatedByUser.ADObjectId));
            var languageId = recipeTranslation.LanguageId;
            Recipe recipe = _dbContext.Recipes
                .Include(i => i.Steps)
                    .ThenInclude(s => s.Translations.Where(t => t.LanguageId == languageId))
                .Include(i => i.Tags)
                    .ThenInclude(s => s.Translations.Where(t => t.LanguageId == languageId))
                .Include(i => i.IngredientLists)
                    .ThenInclude(s => s.Translations.Where(t => t.LanguageId == languageId))
                .Include(i => i.IngredientLists)
                    .ThenInclude(s => s.Ingredients)
                        .ThenInclude(s => s.Ingredient)
                            .ThenInclude(s => s.Translations.Where(t => t.LanguageId == languageId))
                .Include(i => i.IngredientLists)
                    .ThenInclude(s => s.Ingredients)
                        .ThenInclude(s => s.Translations.Where(t => t.LanguageId == languageId))
                .Include(n => n.CalculatedNutrients)
                .Include(i => i.CreatedByUser)
                .Single(_ => _.Id == recipeTranslation.RecipeId && (signedInUserId == _.CreatedByUser.ADObjectId));

            recipe.Translations.Add(recipeTranslation);

            return recipe;
        }
        private double GetIngredientBaseAmount(RecipeIngredient recipeIngredient, List<Measurement> measurements, int servingAmount)
        {
            var currentAmount = recipeIngredient.Amount;
            Ingredient ingredient = _dbContext.Ingredients.AsNoTracking().Single(_ => _.Id == recipeIngredient.IngredientId);

            Measurement recipeIngredientMeasurement = measurements.FirstOrDefault(_ => _.Id == recipeIngredient.MeasurementId);
            Measurement ingredientDefaultNutrientMeasurement = measurements.FirstOrDefault(_ => _.Id == ingredient.DefaultNutrientMeasurementId);
            Measurement ingredientDefaultMeasurement = measurements.FirstOrDefault(_ => _.Id == ingredient.DefaultMeasurementId);

            currentAmount = currentAmount * recipeIngredientMeasurement.ConversionAmount;

            if (recipeIngredientMeasurement.TypeId != ingredientDefaultNutrientMeasurement.TypeId)
            {
                var typeConversionAmount = MeasurementExtensions.GetTypeConversion(ingredient, recipeIngredientMeasurement.TypeId, ingredientDefaultNutrientMeasurement.TypeId);
                currentAmount = currentAmount * typeConversionAmount;
            }

            if (servingAmount == 0) return currentAmount;
            return currentAmount / servingAmount;
        }
        public async Task UpdateRecipeDraftAsync(Recipe updatedRecipe, int languageId)
        {
            List<Measurement> measurements = _dbContext.Measurements.AsNoTracking().ToList();
            Guid signedInUserId = _headerService.GetUserId();
            RecipeTranslation updatedRecipeTranslation = updatedRecipe.Translations.FirstOrDefault();
            Recipe recipe = await GetRecipeDraftTranslationAsync(updatedRecipeTranslation.Id);
            RecipeTranslation recipeTranslation = recipe.Translations.FirstOrDefault();
            if (recipe == null) throw new Exception("Cannot find existing recipe");
            if (signedInUserId != recipe.CreatedByUser.ADObjectId)
            {
                throw new Exception("Cannot match user with recipe");
            }
            if (recipeTranslation.IsPublished)
            {
                throw new Exception("Cannot update recipe, because recipe is already published!");
            }

            // Flavors
            recipe.Sweet = updatedRecipe.Sweet;
            recipe.Bitter = updatedRecipe.Bitter;
            recipe.Flavor = updatedRecipe.Flavor;
            recipe.Salty = updatedRecipe.Salty;
            recipe.Sour = updatedRecipe.Sour;
            recipe.Spices = updatedRecipe.Spices;

            // Video Url
            if (!String.IsNullOrEmpty(updatedRecipe.VideoUrl) && !updatedRecipe.VideoUrl.StartsWith("https://www.youtube.com/embed/"))
            {
                throw new InvalidOperationException("Video Url is not correct");
            }
            else
            {
                recipe.VideoUrl = updatedRecipe.VideoUrl;
            }

            // IngredientLists
            foreach (RecipeIngredientList ingredientList in recipe.IngredientLists.ToList())
            {
                RecipeIngredientList updatedIngredientList = updatedRecipe.IngredientLists.FirstOrDefault(_ => _.Id == ingredientList.Id);
                if (updatedIngredientList == null)
                {
                    _dbContext.Entry(ingredientList).State = EntityState.Deleted;
                }
                else
                {
                    ingredientList.OrderIndex = updatedIngredientList.OrderIndex;
                    RecipeIngredientListTranslation updatedIngredientListTranslation = updatedIngredientList.Translations.FirstOrDefault();
                    RecipeIngredientListTranslation ingredientListTranslation = ingredientList.Translations.FirstOrDefault();
                    ingredientListTranslation.Title = updatedIngredientListTranslation.Title;

                    // Ingredients
                    foreach (RecipeIngredient ingredient in ingredientList.Ingredients.ToList())
                    {
                        try
                        {
                            RecipeIngredient updatedIngredient = updatedIngredientList.Ingredients.SingleOrDefault(_ => _.IngredientId == ingredient.IngredientId);
                            if (updatedIngredient == null)
                            {
                                //_dbContext.Entry(ingredient).State = EntityState.Deleted;
                                ingredientList.Ingredients.Remove(ingredient);
                            }
                            else
                            {
                                ingredient.OrderIndex = updatedIngredient.OrderIndex;
                                ingredient.MeasurementId = updatedIngredient.MeasurementId;
                                ingredient.Amount = updatedIngredient.Amount;
                                RecipeIngredientTranslation updatedIngredientTranslation = updatedIngredient.Translations.FirstOrDefault();
                                RecipeIngredientTranslation ingredienTranslation = ingredient.Translations.FirstOrDefault();
                                ingredienTranslation.Description = updatedIngredientTranslation.Description;
                            }
                        }
                        catch(Exception e)
                        {
                            continue;
                        }
                    }
                    List<RecipeIngredient> ingredientsToAdd = updatedIngredientList.Ingredients.DistinctBy(x => x.IngredientId).ToList().Where(p => ingredientList.Ingredients.All(p2 => p2.Id != p.Id)).ToList();
                    foreach (RecipeIngredient ingredientToAdd in ingredientsToAdd)
                    {
                        ingredientToAdd.Translations.FirstOrDefault().LanguageId = recipe.OriginalLanguageId;
                        ingredientList.Ingredients.Add(ingredientToAdd);
                    }
                }
            }

            List<RecipeIngredientList> ingredientListsToAdd = updatedRecipe.IngredientLists.Where(p => recipe.IngredientLists.All(p2 => p2.Id != p.Id)).ToList();
            foreach (RecipeIngredientList ingredientListToAdd in ingredientListsToAdd)
            {
                ingredientListToAdd.Translations.FirstOrDefault().LanguageId = recipe.OriginalLanguageId;
                recipe.IngredientLists.Add(ingredientListToAdd);
            }

            foreach (var ingredient in recipe.IngredientLists.SelectMany(_ => _.Ingredients))
            {
                ingredient.AmountPerServingDefaultNutrientMeasurement = GetIngredientBaseAmount(ingredient, measurements, updatedRecipe.ServingAmount);
            }
            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync();

            Dictionary<string, int> tempIngredients = new Dictionary<string, int>();
            foreach (var list in recipe.IngredientLists)
            {
                foreach (var tempIngredient in list.Ingredients.Where(_ => _.TempId != null))
                {
                    if (tempIngredients.Keys.ToString() != tempIngredient.TempId)
                    {
                        tempIngredients.Add(tempIngredient.TempId, tempIngredient.Id);
                    }
                }
            }

            //  Steps
            foreach (RecipeStep step in recipe.Steps)
            {
                RecipeStep updatedStep = updatedRecipe.Steps.FirstOrDefault(_ => _.Id == step.Id);
                if (updatedStep == null)
                {
                    if (!String.IsNullOrEmpty(step?.ImageUrl))
                    {
                        await _imageRepository.RemoveImageAsync(step.ImageUrl.Replace("recipe-images/", ""), blobContainerName: "recipe-images");
                    }
                    _dbContext.Entry(step).State = EntityState.Deleted;
                }
                else
                {
                    if (!String.IsNullOrEmpty(updatedStep.ImageUrl) && updatedStep.ImageUrl.StartsWith("data"))
                    {
                        step.ImageUrl = await _imageRepository.StoreImageAsync(updatedStep.ImageUrl, $"{recipe.Id}", maxWidth: 1280, blobContainerName: "recipe-images");
                    }
                    if (!String.IsNullOrEmpty(updatedStep.VideoUrl) && !updatedStep.VideoUrl.StartsWith("https://www.youtube.com/embed/"))
                    {
                        throw new InvalidOperationException("Video Url is not correct");
                    }
                    else
                    {
                        step.VideoUrl = updatedStep.VideoUrl;
                    }
                    step.OrderIndex = updatedStep.OrderIndex;
                    RecipeStepTranslation updatedStepTranslation = updatedStep.Translations.FirstOrDefault();
                    RecipeStepTranslation stepTranslation = step.Translations.FirstOrDefault();
                    stepTranslation.Title = updatedStepTranslation.Title;
                    /* update step content */
                    if (tempIngredients.Count() > 0)
                    {
                        string content = updatedStepTranslation.Content;
                        foreach (var tempIngredient in tempIngredients)
                        {
                            content = content.Replace(tempIngredient.Key.ToString(), tempIngredient.Value.ToString());
                        }
                        stepTranslation.Content = content;
                    }
                    else
                    {
                        stepTranslation.Content = updatedStepTranslation.Content;
                    }

                }
            }
            List<RecipeStep> stepsToAdd = updatedRecipe.Steps.Where(p => recipe.Steps.All(p2 => p2.Id != p.Id)).ToList();
            foreach (RecipeStep stepToAdd in stepsToAdd)
            {
                if (!String.IsNullOrEmpty(stepToAdd.ImageUrl) && stepToAdd.ImageUrl.StartsWith("data"))
                {
                    stepToAdd.ImageUrl = await _imageRepository.StoreImageAsync(stepToAdd.ImageUrl, $"{recipe.Id}", maxWidth: 1280, blobContainerName: "recipe-images");
                }
                if (!String.IsNullOrEmpty(stepToAdd.VideoUrl) && !stepToAdd.VideoUrl.StartsWith("https://www.youtube.com/embed/"))
                {
                    throw new InvalidOperationException("Video Url is not correct");
                }
                stepToAdd.Translations.FirstOrDefault().LanguageId = recipe.OriginalLanguageId;
                /* update step content */
                if (tempIngredients.Count() > 0)
                {
                    string content = stepToAdd.Translations.FirstOrDefault().Content;
                    foreach (var tempIngredient in tempIngredients)
                    {
                        content = content.Replace(tempIngredient.Key.ToString(), tempIngredient.Value.ToString());
                        stepToAdd.Translations.FirstOrDefault().Content = content;
                    }
                }
                recipe.Steps.Add(stepToAdd);
            }

            // recipe usertags
            //if (updatedRecipe.UserTags.Count() > 0 || recipe.UserTags.Count > 0)
            //{
            //    ICollection<UserTag> userTags = await _userTagRepository.GetOrCreateUserTagsAsync(updatedRecipe.UserTags.Distinct().ToList(), recipe.OriginalLanguageId);
            //    List<int> updatedUserTagIds = userTags.Select(_ => _.Id).ToList();
            //    List<int> existingUserTagIds = recipe.UserTags.Select(_ => _.Id).ToList();
            //    List<UserTag> userTagsToRemove = recipe.UserTags.Where(t => !updatedUserTagIds.Contains(t.Id)).ToList();
            //    List<UserTag> userTagsToAdd = userTags.Where(t => !existingUserTagIds.Contains(t.Id)).ToList();

            //    foreach (UserTag userTagToRemove in userTagsToRemove)
            //    {
            //        recipe.UserTags.Remove(userTagToRemove);
            //    }
            //    foreach (UserTag userTagToAdd in userTagsToAdd)
            //    {
            //        recipe.UserTags.Add(userTagToAdd);
            //    }
            //}

            // recipe image
            if (!String.IsNullOrEmpty(updatedRecipe.ImageUrl) && updatedRecipe.ImageUrl.StartsWith("data"))
            {
                await _imageRepository.StoreImageAsync(updatedRecipe.ImageUrl, $"{recipe.Id}", name: "mainThumb", maxWidth: 300, blobContainerName: "recipe-images");
                var imagePath = await _imageRepository.StoreImageAsync(updatedRecipe.ImageUrl, $"{recipe.Id}", name: "main", maxWidth: 1280, blobContainerName: "recipe-images");
                recipe.ImageUrl = imagePath.ReplaceLast("/main", "/");
            }
            else if (!String.IsNullOrEmpty(updatedRecipe.ImageUrl) && updatedRecipe.ImageUrl != $"/recipe-images/{recipe.Id}/")
            {
                throw new InvalidOperationException("Image path is not correct");
            }

            // recipe
            recipe.TotalTime = (updatedRecipe.TotalPrepTime + updatedRecipe.TotalCookingTime);
            recipe.TotalPrepTime = updatedRecipe.TotalPrepTime;
            recipe.TotalCookingTime = updatedRecipe.TotalCookingTime;
            recipe.DifficultyId = updatedRecipe.DifficultyId;
            recipe.BestServedId = updatedRecipe.BestServedId;
            recipe.ServingAmount = updatedRecipe.ServingAmount;
            recipe.OriginCountryId = updatedRecipe.OriginCountryId;
            recipe.UpdatedAt = DateTime.Now;

            // translation
            recipeTranslation.UpdatedAt = DateTime.Now;
            recipeTranslation.Title = updatedRecipeTranslation.Title;
            recipeTranslation.Summary = updatedRecipeTranslation.Summary;


            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task PublishRecipeDraftAsync(Guid id)
        {
            Guid signedInUserId = _headerService.GetUserId();

            // Get recipe translation
            RecipeTranslation recipeTranslation = _dbContext.RecipeTranslations
                .Include(i => i.CreatedByUser)
                .Where(_ => _.CreatedByUser.ADObjectId == signedInUserId)
                .Single(_ => _.Id == id);

            // Get Recipe
            Recipe recipe = _dbContext.Recipes.Where(_ => _.Id == recipeTranslation.RecipeId)
                .Include(i => i.IngredientLists)
                    .ThenInclude(s => s.Ingredients)
                        .ThenInclude(s => s.Ingredient)
                            .ThenInclude(_ => _.Nutrients)
                                .ThenInclude(_ => _.Nutrient)
                .Include(i => i.IngredientLists)
                    .ThenInclude(s => s.Ingredients)
                        .ThenInclude(s => s.Ingredient)
                            .ThenInclude(_ => _.Tags)
                .Include(i => i.Tags)
                .Include(i => i.Steps)
                .Single(_ => _.Id == recipeTranslation.RecipeId);


            recipe.UpdatedAt = DateTime.Now;
            recipeTranslation.IsPublished = true;
            recipeTranslation.PublishedAt = DateTime.Now;
            recipeTranslation.UpdatedAt = DateTime.Now;
            //await CalculateRecipeNutrientsAsync(recipe);
            //await AddAdditionalTags(recipe);
            await PrepareRecipeApproval(recipe);
            _dbContext.Recipes.Update(recipe);
            _dbContext.RecipeTranslations.Update(recipeTranslation);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }
        public async Task ApproveRecipeDraftAsync(Guid id, bool checkIngredients)
        {
            // Get recipe translation
            RecipeTranslation recipeTranslation = _dbContext.RecipeTranslations.Single(_ => _.Id == id);

            // Get Recipe
            Recipe recipe = _dbContext.Recipes.Where(_ => _.Id == recipeTranslation.RecipeId)
                .Include(i => i.IngredientLists)
                    .ThenInclude(s => s.Ingredients)
                        .ThenInclude(s => s.Ingredient)
                            .ThenInclude(_ => _.Nutrients)
                                .ThenInclude(_ => _.Nutrient)
                .Include(i => i.IngredientLists)
                    .ThenInclude(s => s.Ingredients)
                        .ThenInclude(s => s.Ingredient)
                            .ThenInclude(_ => _.Tags)
                .Include(i => i.Tags)
                .Single(_ => _.Id == recipeTranslation.RecipeId);

            if(checkIngredients)
            {
                foreach (RecipeIngredient ingredient in recipe.IngredientLists.SelectMany(_ => _.Ingredients))
                {
                    Ingredient dbIngredient = _dbContext.Ingredients.AsNoTracking().Single(_ => _.Id == ingredient.IngredientId);
                    if (!dbIngredient.Published)
                    {
                        throw new Exception("Cannot approve recipe, because of unpublished ingredients");
                    }
                }
            }

            recipe.UpdatedAt = DateTime.Now;
            recipeTranslation.IsApproved = true;
            recipeTranslation.ApprovedAt = DateTime.Now;
            recipeTranslation.UpdatedAt = DateTime.Now;
            await CalculateRecipeNutrientsAsync(recipe);
            await AddAdditionalTags(recipe);
            _dbContext.Recipes.Update(recipe);
            _dbContext.RecipeTranslations.Update(recipeTranslation);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }
        public async Task AddAdditionalTags(Recipe recipe)
        {
            List<RecipeIngredient> recipeIngredients = recipe.IngredientLists.SelectMany(_ => _.Ingredients).ToList();
            List<Tag> IngredientTags = recipeIngredients.SelectMany(c => c.Ingredient.Tags.Where(t => t.ExcludeAlways != true)).ToList();
            List<Tag> uniqueIngredientTags = IngredientTags.DistinctBy(x => x.Id).ToList();
            List<int> existingRecipeTagIds = recipe.Tags.Select(_ => _.Id).ToList();
            int totalIngredients = recipeIngredients.Count();
            List<Tag> allTags = new List<Tag>();
            foreach (Tag tag in uniqueIngredientTags.Where(t => !existingRecipeTagIds.Contains(t.Id)).ToList())
            {
                if (tag.ExcludeAlways == true)
                {
                    continue;
                }
                else if (tag.IncludeAlways == true)
                {
                    recipe.Tags.Add(tag);
                }
                else
                {
                    int TotalFoundTagInIngredients = IngredientTags.Where(_ => _.Id == tag.Id).Count();
                    if (totalIngredients == TotalFoundTagInIngredients)
                    {
                        recipe.Tags.Add(tag);
                    }
                }
            }

            foreach (RecipeCalculatedNutrient nutrient in recipe.CalculatedNutrients)
            {
                if (nutrient.NutrientId == 25 && nutrient.Amount > 0 && recipe.Tags.Any(_ => _.Id != 1471)) /* Alchohol */
                {
                    var newtag = new Tag()
                    {
                        Id = 1471
                    };
                    recipe.Tags.Add(newtag);
                }
                else if (nutrient.NutrientId == 35 && nutrient.Amount > 0 && recipe.Tags.Any(_ => _.Id != 1752)) /* Sugar - Lactose */
                {
                    var newtag = new Tag()
                    {
                        Id = 1752
                    };
                    recipe.Tags.Add(newtag);
                }
            }
        }
        public async Task CalculateRecipeNutrientsAsync(Recipe recipe)
        {
            List<RecipeCalculatedNutrient> recipeNutrients = new List<RecipeCalculatedNutrient>();
            List<Measurement> measurements = _dbContext.Measurements.AsNoTracking().ToList();

            foreach (RecipeIngredient ingredient in recipe.IngredientLists.SelectMany(_ => _.Ingredients))
            {
                Ingredient dbIngredient = _dbContext.Ingredients.AsNoTracking().Single(_ => _.Id == ingredient.IngredientId);
                foreach (IngredientNutrient ingredientNutrient in ingredient.Ingredient.Nutrients.Where(n => n.Nutrient.IsCommon))
                {
                    var nutrientAmount = ingredientNutrient.Amount;
                    var convertedNutrientAmount = nutrientAmount * ingredient.AmountPerServingDefaultNutrientMeasurement;

                    RecipeCalculatedNutrient nutrient = new RecipeCalculatedNutrient()
                    {
                        NutrientId = ingredientNutrient.NutrientId,
                        RecipeId = recipe.Id,
                        Amount = convertedNutrientAmount,
                    };

                    var foundNutrient = recipeNutrients.FirstOrDefault(_ => _.NutrientId == nutrient.NutrientId);
                    if (foundNutrient != null)
                    {
                        foundNutrient.Amount = foundNutrient.Amount + nutrient.Amount; /* combines nutrients together */
                    }
                    else
                    {
                        if (nutrient.Amount != 0)
                        {
                            recipeNutrients.Add(nutrient);
                        }
                    }

                }
            }

            List<RecipeCalculatedNutrient> foundRecipeNutrients = await _dbContext.RecipeCalculatedNutrients.Where(_ => _.RecipeId == recipe.Id).ToListAsync();

            foreach (RecipeCalculatedNutrient foundRecipeNutrient in foundRecipeNutrients)
            {
                var test = recipeNutrients.FirstOrDefault(_ => _.NutrientId == foundRecipeNutrient.NutrientId);
                if (test != null)
                {
                    foundRecipeNutrient.Amount = test.Amount;
                    recipeNutrients.Remove(test);
                }
                else
                {
                    _dbContext.Entry(foundRecipeNutrient).State = EntityState.Deleted;
                }
            }
            if (recipeNutrients.Count > 0)
            {
                _dbContext.RecipeCalculatedNutrients.AddRange(recipeNutrients.Where(x => !foundRecipeNutrients.Any(y => y.NutrientId == x.NutrientId)));
            }
        }
        public async Task PrepareRecipeApproval(Recipe recipe)
        {
            int unPublishedIngredients = 0;
            int allIngredients = 0;
            var unPublishedIngredientNames = "";
            var unPublishedIngredientIds = "";
            foreach (RecipeIngredient ingredient in recipe.IngredientLists.SelectMany(_ => _.Ingredients))
            {
                allIngredients++;
                Ingredient dbIngredient = _dbContext.Ingredients.AsNoTracking().Single(_ => _.Id == ingredient.IngredientId);
                if(!dbIngredient.Published)
                {
                    unPublishedIngredients++;
                    unPublishedIngredientIds += dbIngredient.Id + ";";
                    unPublishedIngredientNames += dbIngredient.EnglishName + ";";
                }
            }

            var recipeApproval = new RecipeApproval()
            {
                CreatedAt = DateTime.Now,
                RecipeId = recipe.Id,
                CreatedById = recipe.Translations.FirstOrDefault().CreatedByUserId,
                RecipeName = recipe.Translations.FirstOrDefault().Title,
                LanguageId = recipe.Translations.FirstOrDefault().LanguageId,
                RecipeTranslationId = recipe.Translations.FirstOrDefault().Id,
                UnPublishedIngredients = unPublishedIngredients,
                AmountOfSteps = recipe.Steps.Count(),
                AmountOfIngredients = allIngredients,
                UnPublishedIngredientIds = unPublishedIngredientIds,
                UnPublishedIngredientNames = unPublishedIngredientNames
            };
            _dbContext.RecipeApprovals.Add(recipeApproval);
        }

        public async Task<List<Recipe>> GetRecipeDraftTranslationsAsync()
        {
            Guid signedInUserId = _headerService.GetUserId();
            int languageId = _headerService.GetLanguageId().GetValueOrDefault();
            List<RecipeTranslation> recipeTranslations = await _dbContext.RecipeTranslations.Include(i => i.CreatedByUser).Where(_ => signedInUserId == _.CreatedByUser.ADObjectId).ToListAsync();

            List<int> RecipeIds = recipeTranslations.Select(_ => _.RecipeId).ToList();

            List<Recipe> recipes = _dbContext.Recipes
                .Where(i => RecipeIds.Contains(i.Id))
                .Include(i => i.CreatedByUser)
                //.Include(i => i.IngredientLists)
                //    .ThenInclude(i => i.Ingredients)
                //        .ThenInclude(i => i.Ingredient)
                //.Include(i => i.IngredientLists)
                //    .ThenInclude(i => i.Ingredients)
                //        .ThenInclude(i => i.Translations.Where(_ => _.LanguageId == languageId))
                .Include(i => i.Tags)
                    .ThenInclude(s => s.Translations.Where(t => t.LanguageId == languageId))
                .Include(i => i.IngredientLists)
                    .ThenInclude(s => s.Translations.Where(t => t.LanguageId == languageId))
                .Include(i => i.IngredientLists)
                    .ThenInclude(s => s.Ingredients)
                        .ThenInclude(s => s.Ingredient)
                            .ThenInclude(s => s.Translations.Where(t => t.LanguageId == languageId))
                .Include(i => i.IngredientLists)
                    .ThenInclude(s => s.Ingredients)
                        .ThenInclude(s => s.Translations.Where(t => t.LanguageId == languageId))
                .Include(n => n.CalculatedNutrients)
                .ToList();
            foreach (Recipe recipe in recipes)
            {
                recipe.Translations = recipeTranslations.Where(_ => _.RecipeId == recipe.Id).ToList();
            }
            return recipes;

        }

        public async Task<ActionResult> DeleteRecipeAsync(int id)
        {
            Recipe item = await _dbContext.Recipes.Include(_ => _.Translations).FirstOrDefaultAsync(_ => _.Id == id).ConfigureAwait(false);
            if (item == null) throw new Exception("No recipe could be found with id: " + id);
            if (item?.Translations.FirstOrDefault(_ => _.IsPublished == true).IsPublished == true) throw new Exception("Cannot delete recipe, that has a published translation.");
            await _imageRepository.RemoveImageFolderAsync($"{item.Id}", blobContainerName: "recipe-images");
            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return new EmptyResult();
        }
    }
}
