using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Models;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services.Interfaces;

namespace WorldsBelly.DataAccess.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IHeaderService _headerService;
        private IUserRepository _userRepository;

        public RecipeRepository(AppDbContext dbContext, IHeaderService headerService, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _headerService = headerService;
            _userRepository = userRepository;
        }

        public Task<Recipe> CreateRecipeDraftAsync(Recipe recipe)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Recipe> GetRecipeTranslationAsync(Guid id)
        {
            RecipeTranslation recipeTranslation = _dbContext.RecipeTranslations.AsNoTracking().Include(i => i.CreatedByUser).Single(_ => _.Id == id);
            var languageId = recipeTranslation.LanguageId;
            Recipe recipe = _dbContext.Recipes
                .Where(_ => _.Id == recipeTranslation.RecipeId)
                //.Include(i => i.Steps)
                //    .ThenInclude(s => s.Translations.Where(t => t.LanguageId == languageId))
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
                .Include(i => i.CreatedByUser)
                .Include(n => n.CalculatedNutrients)
                .Single(_ => _.Id == recipeTranslation.RecipeId);

            recipe.Translations.Add(recipeTranslation);

            return recipe;
        }
        public async Task<ICollection<RecipeStep>> GetRecipeStepsTranslationAsync(Guid id)
        {
            RecipeTranslation recipeTranslation = _dbContext.RecipeTranslations.AsNoTracking().Single(_ => _.Id == id);
            var languageId = recipeTranslation.LanguageId;
            var steps = await _dbContext.RecipeSteps
                .Where(_ => _.RecipeId == recipeTranslation.RecipeId)
                .Include(s => s.Translations.Where(t => t.LanguageId == languageId)).ToListAsync();
            try
            {
                var recipe = await _dbContext.Recipes.FirstOrDefaultAsync(_ => _.Id == recipeTranslation.RecipeId);
                if(recipe != null)
                {
                    recipe.TotalViews = recipe.TotalViews + 1;
                    _dbContext.Update(recipe);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                return steps;
            }
            return steps;

        }

        public IQueryable<Recipe> GetRecipes(RecipeFilterQuery filter)
        {
            int languageId = _headerService.GetLanguageId().GetValueOrDefault();
            var toSkip = (filter.Page - 1) * filter.PageSize;

            IQueryable<Recipe> query = _dbContext.Recipes.AsNoTracking();

            query = query
                .Include(t => t.Translations.Where(_ => _.LanguageId == languageId))
                        //.ThenInclude(c => c.CreatedByUser)
                .Include(i => i.CreatedByUser)
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
            .Where(_ => _.Translations.All(x => x.IsApproved && x.LanguageId == languageId));

            if(!String.IsNullOrEmpty(filter.SortBy))
            {
                var sortBy = filter.SortBy;
                var isAsc = filter.SortAscending;
                if (sortBy == "Newest")
                {
                    query = isAsc ? 
                        query.OrderByDescending(x => x.Translations.FirstOrDefault().PublishedAt) :
                        query.OrderBy(x => x.Translations.FirstOrDefault().PublishedAt);
                }
                else if (sortBy == "TotalTime")
                {
                    query = isAsc ?
                        query.OrderBy(x => x.TotalTime) :
                        query.OrderByDescending(x => x.TotalTime);
                }
                else if (sortBy == "TotalViews")
                {
                    query = isAsc ?
                        query.OrderByDescending(x => x.TotalViews) :
                        query.OrderBy(x => x.TotalViews);
                }
                else if (sortBy == "TotalLikes")
                {
                    query = isAsc ?
                        query.OrderByDescending(x => x.CalculatedTotaliked) :
                        query.OrderBy(x => x.CalculatedTotaliked);
                }
                else if (sortBy == "TotalRating")
                {
                    query = isAsc ?
                        query.OrderByDescending(x => x.CalculatedTotalRatings) :
                        query.OrderBy(x => x.CalculatedTotalRatings);
                }
                else if (sortBy == "Rating")
                {
                    query = isAsc ?
                        query.OrderByDescending(x => x.CalculatedRating > 2)
                            .ThenBy(x => x.CalculatedRating) :
                        query.OrderBy(x => x.CalculatedRating > 2)
                            .ThenBy(x => x.CalculatedRating);
                }
                else
                {
                    query = isAsc ?
                        query.OrderByDescending(x => (x.CalculatedRating * x.CalculatedTotaliked) / x.TotalViews)
                            .ThenBy(x => x.CreatedAt) :
                        query.OrderBy(x => (x.CalculatedRating * x.CalculatedTotaliked) / x.TotalViews)
                            .ThenBy(x => x.CreatedAt);
                }
            }

            if (!String.IsNullOrWhiteSpace(filter.Search))
            {
                string[] searchArray = filter.Search.ToLower().Trim().Split(" ");
                foreach (string searchWord in searchArray.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    query = query.Where(i =>
                        i.Translations.SingleOrDefault().Title.ToLower().Contains(searchWord.Trim()) ||
                        i.Tags.Any(t => t.Translations.SingleOrDefault().Name.ToLower().Trim() == searchWord.Trim()) ||
                        i.Tags.Any(t => t.Translations.SingleOrDefault().NamePlural.ToLower().Trim() == searchWord.Trim())
                    );
                }
            }
            if (filter.Tags != null)
            {
                foreach (var fTag in filter.Tags)
                {
                    query = query.Where(i => i.Tags.SingleOrDefault(x => x.Id == fTag) != null);
                }
            }
            if (filter.BestServed != null)
            {
                query = query.Where(i => filter.BestServed.Contains(i.BestServedId.GetValueOrDefault()));
            }
            if (filter.Difficulty != null)
            {
                query = query.Where(i => filter.Difficulty.Contains(i.DifficultyId.GetValueOrDefault()));
            }
            if (filter.Includeingredients != null)
            {
                query = query.Where(i => i.IngredientLists.Any(l => l.Ingredients.Any(t => filter.Includeingredients.Contains(t.IngredientId))));
            }
            if (filter.Excludeingredients != null)
            {
                query = query.Where(i => i.IngredientLists.All(l => l.Ingredients.All(t => !filter.Excludeingredients.Contains(t.IngredientId))));
            }
            if (filter.Nutrients != null)
            {
                foreach (var fNutrient in filter.Nutrients)
                {
                    if (fNutrient.Option == Option.MoreThan)
                    {
                        query = query.Where(t => t.CalculatedNutrients.SingleOrDefault(n => n.NutrientId == fNutrient.Id).Amount >= fNutrient.Amount);
                    }
                    else
                    {
                        query = query.Where(t => t.CalculatedNutrients.SingleOrDefault(n => n.NutrientId == fNutrient.Id).Amount <= fNutrient.Amount);
                    }
                }
            }
            if(filter.Time != null)
            {
                if (filter.Time.Option == Option.MoreThan)
                {
                    query = query.Where(t => t.TotalTime >= filter.Time.Amount);
                }
                else
                {
                    query = query.Where(t => t.TotalTime <= filter.Time.Amount);
                }
            }
            if (filter.Includecuisines != null)
            {
                query = query.Where(_ => filter.Includecuisines.Contains(_.OriginCountryId.GetValueOrDefault()));
            }
            if (filter.Excludecuisines != null)
            {
                query = query.Where(_ => !filter.Excludecuisines.Contains(_.OriginCountryId.GetValueOrDefault()));
            }
            if (filter.Fromrating != null || filter.Torating != null)
            {
                if (filter.Fromrating == null) filter.Fromrating = 0;
                if (filter.Torating == null) filter.Torating = 10;
                query = query.Where(i => filter.Fromrating <= i.CalculatedRating && filter.Torating >= i.CalculatedRating);
            }
            if (filter.Fromsweet != null || filter.Tosweet != null)
            {
                if (filter.Fromsweet == null) filter.Fromsweet = 0;
                if (filter.Tosweet == null) filter.Tosweet = 5;
                query = query.Where(i => filter.Fromsweet <= i.Sweet && filter.Tosweet >= i.Sweet);
            }
            if (filter.Fromsour != null || filter.Tosour != null)
            {
                if (filter.Fromsour == null) filter.Fromsour = 0;
                if (filter.Tosour == null) filter.Tosour = 5;
                query = query.Where(i => filter.Fromsour <= i.Sour && filter.Tosour >= i.Sour);
            }
            if (filter.Fromsalty != null || filter.Tosalty != null)
            {
                if (filter.Fromsalty == null) filter.Fromsalty = 0;
                if (filter.Tosalty == null) filter.Tosalty = 5;
                query = query.Where(i => filter.Fromsalty <= i.Salty && filter.Tosalty >= i.Salty);
            }
            if (filter.Frombitter != null || filter.Tobitter != null)
            {
                if (filter.Frombitter == null) filter.Frombitter = 0;
                if (filter.Tobitter == null) filter.Tobitter = 5;
                query = query.Where(i => filter.Frombitter <= i.Bitter && filter.Tobitter >= i.Bitter);
            }
            if (filter.Fromspices != null || filter.Tospices != null)
            {
                if (filter.Fromspices == null) filter.Fromspices = 0;
                if (filter.Tospices == null) filter.Tospices = 5;
                query = query.Where(i => filter.Fromspices <= i.Spices && filter.Tospices >= i.Spices);
            }
            if (filter.Fromflavor != null || filter.Toflavor != null)
            {
                if (filter.Fromflavor == null) filter.Fromflavor = 0;
                if (filter.Toflavor == null) filter.Toflavor = 5;
                query = query.Where(i => filter.Fromflavor <= i.Flavor && filter.Toflavor >= i.Flavor);
            }

            query = query.Skip(toSkip).Take(filter.PageSize);
            return query;
        }


        public IQueryable<Recipe> GetRecipesRelatedToUser(RecipeRelatedToUserFilterQuery filter)
        {
            int languageId = _headerService.GetLanguageId().GetValueOrDefault();
            var toSkip = (filter.Page - 1) * filter.PageSize;

            IQueryable<Recipe> query = _dbContext.Recipes.AsNoTracking();

            query = query
                .Include(t => t.Translations.Where(_ => _.LanguageId == languageId))
                        //.ThenInclude(c => c.CreatedByUser)
                .Include(i => i.CreatedByUser)
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
            .Where(_ => _.Translations.All(x => x.IsApproved && x.LanguageId == languageId));

            if(filter.CreatedBy != null)
            {
                query = query.Where(_ => _.CreatedByUser.Id == filter.CreatedBy);
            }
            if (filter.LikedBy != null)
            {
                query = query.Include(_ => _.Likes).Where(r => r.Likes.Any(l => l.UserId == filter.LikedBy));
            }
            if (filter.ReviewedBy != null)
            {
                query = query.Include(_ => _.Ratings).Where(r => r.Ratings.Any(l => l.UserId == filter.ReviewedBy));
            }

            query = query.Skip(toSkip).Take(filter.PageSize);
            return query;
        }
        public async Task<ICollection<RecipeBestServed>> GetRecipeBestServedAsync()
        {
            int? languageId = _headerService.GetLanguageId();
            if(languageId != null)
            {
                return await _dbContext.RecipeBestServed.AsNoTracking().Include(_ => _.Tag).ThenInclude(t => t.Translations.Where(t => t.LanguageId == languageId)).ToListAsync();
            }
            return await _dbContext.RecipeBestServed.Include(_ => _.Tag).ToListAsync();
        }
        public async Task<ICollection<RecipeConsumer>> GetRecipeConsumerAsync()
        {
            int? languageId = _headerService.GetLanguageId();
            if (languageId != null)
            {
                return await _dbContext.RecipeConsumer.AsNoTracking().Include(_ => _.Tag).ThenInclude(t => t.Translations.Where(t => t.LanguageId == languageId)).ToListAsync();
            }
            return await _dbContext.RecipeConsumer.Include(_ => _.Tag).ToListAsync();
        }
        public async Task<ICollection<RecipeAgeGroup>> GetRecipeAgeGroupAsync()
        {
            int? languageId = _headerService.GetLanguageId();
            if (languageId != null)
            {
                return await _dbContext.RecipeAgeGroup.AsNoTracking().Include(_ => _.Tag).ThenInclude(t => t.Translations.Where(t => t.LanguageId == languageId)).ToListAsync();
            }
            return await _dbContext.RecipeAgeGroup.Include(_ => _.Tag).ToListAsync();
        }
        public async Task<ICollection<RecipeDifficulty>> GetRecipeDifficultyAsync()
        {
            int? languageId = _headerService.GetLanguageId();
            if (languageId != null)
            {
                return await _dbContext.RecipeDifficulty.AsNoTracking().Include(_ => _.Tag).ThenInclude(t => t.Translations.Where(t => t.LanguageId == languageId)).ToListAsync();
            }
            return await _dbContext.RecipeDifficulty.Include(_ => _.Tag).ToListAsync();
        }
        public async Task<RecipeRating> GetRecipeUserRatingAsync(int recipeId)
        {
            Guid signedInUserId = _headerService.GetUserId();
            User user = await _userRepository.GetUserByAzureIdAsync(signedInUserId);
            return _dbContext.RecipeRatings.Include(_ => _.User).SingleOrDefault(_ => _.UserId == user.Id && _.RecipeId == recipeId);
        }
        public async Task CreateRecipeUserRatingAsync(RecipeRating rating)
        {
            Recipe recipe = await _dbContext.Recipes.FirstOrDefaultAsync(_ => _.Id == rating.RecipeId);
            if(recipe.CreatedByUserId == rating.UserId)
            {
                throw new Exception("You cannot rate your own recipe!");
            }
            _dbContext.RecipeRatings.Add(rating);
            await _dbContext.SaveChangesAsync();
            await CalculateRecipeRatingAsync(rating.RecipeId);
        }
        public async Task RemoveRecipeUserRatingAsync(RecipeRating rating)
        {
            _dbContext.RecipeRatings.Remove(rating);
            await _dbContext.SaveChangesAsync();
            await CalculateRecipeRatingAsync(rating.RecipeId);
        }
        public async Task UpdateRecipeUserRatingAsync(RecipeRating rating)
        {
            _dbContext.RecipeRatings.Update(rating);
            await _dbContext.SaveChangesAsync();
            await CalculateRecipeRatingAsync(rating.RecipeId);
        }
        public async Task CalculateRecipeRatingAsync(int recipeId)
        {
            Recipe recipe = await _dbContext.Recipes.FirstOrDefaultAsync(_ => _.Id == recipeId);
            List<RecipeRating> recipeRatings = _dbContext.RecipeRatings.AsNoTracking().Where(_ => _.RecipeId == recipeId).ToList();

            recipe.CalculatedTotalRatings = recipeRatings.Count();
            recipe.CalculatedRating = recipeRatings.Count() == 0 ? 0 : recipeRatings.Average(x => x.Rating);

            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync();
        }

        public async Task LikeRecipeAsync(int recipeId)
        {
            Guid signedInUserId = _headerService.GetUserId();
            User user = await _userRepository.GetUserByAzureIdAsync(signedInUserId);

            var recipeLike = await _dbContext.RecipeLikes.FirstOrDefaultAsync(_ => _.RecipeId == recipeId && _.UserId == user.Id);
            Recipe recipe = await _dbContext.Recipes.FirstOrDefaultAsync(_ => _.Id == recipeId);

            if (recipeLike == null)
            {
                _dbContext.RecipeLikes.Add(new RecipeLike { 
                    RecipeId = recipeId,
                    UserId = user.Id
                });
                recipe.CalculatedTotaliked = recipe.CalculatedTotaliked + 1;
            }
            else
            {
                _dbContext.RecipeLikes.Remove(recipeLike);
                recipe.CalculatedTotaliked = recipe.CalculatedTotaliked - 1;
                if (recipe.CalculatedTotaliked < 0) recipe.CalculatedTotaliked = 0;
                _dbContext.Recipes.Update(recipe);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> GetLikeRecipeAsync(int recipeId)
        {
            Guid signedInUserId = _headerService.GetUserId();
            User user = await _userRepository.GetUserByAzureIdAsync(signedInUserId);

            var recipeLike = await _dbContext.RecipeLikes.AsNoTracking().FirstOrDefaultAsync(_ => _.RecipeId == recipeId && _.UserId == user.Id);

            if (recipeLike == null)
            {
                return false;
            }
            return true;
        }
    }
}
