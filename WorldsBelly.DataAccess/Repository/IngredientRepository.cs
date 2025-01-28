using Microsoft.EntityFrameworkCore;
using static MoreLinq.Extensions.LagExtension;
using static MoreLinq.Extensions.LeadExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Entities.Filters;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;
using WorldsBelly.Domain.Utils.Helpers;

namespace WorldsBelly.DataAccess.Repository
{
	public class IngredientRepository : IIngredientRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IHeaderService _headerService;
        private IImageRepository _imageRepository;

        public IngredientRepository(AppDbContext dbContext, IHeaderService headerService, IImageRepository imageRepository)
        {
            _dbContext = dbContext;
            _headerService = headerService;
            _imageRepository = imageRepository;
        }

        public async Task<Ingredient> CreateIngredientAsync(Ingredient entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.TranslationsAmount = 1;
            _dbContext.Ingredients.Add(entity);
            await _dbContext.SaveChangesAsync();


            IngredientTranslation englishTranslation = new IngredientTranslation()
            {
                IngredientId = entity.Id,
                LanguageId = 20,
                Name = entity.EnglishName,
                NamePlural = entity.EnglishNamePlural,
                Description = entity.EnglishDescription
            };

            _dbContext.IngredientTranslations.Add(englishTranslation);
            await _dbContext.SaveChangesAsync();

            return await GetIngredientAsync(entity.Id);
        }

        public async Task<Ingredient> GetIngredientAsync(int id)
		{
			return _dbContext.Ingredients
                .Where(_ => _.Id == id)
                .Include(i => i.DefaultMeasurement)
                .Include(i => i.DefaultNutrientMeasurement)
                .Include(i => i.Nutrients)
                .Include(i => i.Tags)
                .Include(i => i.Translations)
                .FirstOrDefault();
		}

		public IQueryable<Ingredient> GetIngredientsQuery(SortingOptions sortingOptions)
        {
            return _dbContext.Ingredients.AsNoTracking()
                .Where(_ => _.Archived != true)
                .OrderBy(a => a.EnglishName.Length)
                //.Include(i => i.Nutrients)
                .Include(i => i.Tags)
                //.Include(i => i.Translations)
                .ApplySorting(sortingOptions);
        }

        public IQueryable<Ingredient> GetIngredientsWithTags()
        {
            return _dbContext.Ingredients
                .Include(i => i.Tags);
        }

        public async Task DeleteIngredientsAsync(List<int> ingredientIds)
        { 
            foreach(var ingredientId in ingredientIds)
            {
                var item = await GetIngredientAsync(ingredientId).ConfigureAwait(false);
                if (!String.IsNullOrEmpty(item.ImageUrl))
                {
                    await _imageRepository.RemoveImageFolderAsync($"{item.Id}", blobContainerName: "ingredient-images");
                }

                _dbContext.Remove(item);
            }
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }
        public async Task ArchiveIngredientsAsync(List<int> ingredientIds)
        {
            foreach (var ingredientId in ingredientIds)
            {
                var item = await GetIngredientAsync(ingredientId).ConfigureAwait(false);
                if (!String.IsNullOrEmpty(item.ImageUrl))
                {
                    await _imageRepository.RemoveImageFolderAsync($"{item.Id}", blobContainerName: "ingredient-images");
                }

                item.Archived = true;
                item.Translations = null;
                item.TranslationsAmount = null;
                item.Nutrients = null;
                item.NutrientsAmount = null;
                _dbContext.Update(item);
            }
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        public async Task RemoveDuplicatedIngredientsAsync()
        {
            var ingredients = await _dbContext.Ingredients.OrderBy(a => a.EnglishName.Length).ToListAsync();
            var removedIngredients = new List<Ingredient>();
            foreach (var ingredient in ingredients.ToList())
            {
                if (removedIngredients.FirstOrDefault(_ => _.Id == ingredient.Id) != null)
                {
                    continue;
                }
                var duplicatedIngredients = ingredients.Where(_ => _.EnglishName.Trim().ToLower() == ingredient.EnglishName.Trim().ToLower() && _.Id != ingredient.Id);
                if (duplicatedIngredients.Count() > 0)
                {
                    removedIngredients.AddRange(duplicatedIngredients);
                    _dbContext.RemoveRange(duplicatedIngredients);
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task UpdateIngredientsEnglishtranslations()
        {
            var ingredientTranslations = _dbContext.IngredientTranslations.Where(x => x.LanguageId == 20);
            var ingredients = _dbContext.Ingredients.Where(x => !ingredientTranslations.Any(y => y.IngredientId == x.Id));
            var existingIngredients = _dbContext.Ingredients.Where(x => ingredientTranslations.Any(y => y.IngredientId == x.Id));
            foreach (IngredientTranslation ingredientTranslation in ingredientTranslations)
            {
                var ingredient = existingIngredients.FirstOrDefault(_ => _.Id == ingredientTranslation.IngredientId);
                ingredientTranslation.Name = ingredient.EnglishName;
                ingredientTranslation.NamePlural = ingredient.EnglishNamePlural;
                ingredientTranslation.Description = ingredient.EnglishDescription;
                _dbContext.IngredientTranslations.Update(ingredientTranslation);
            }
            foreach(Ingredient ingredient in ingredients)
            {
                _dbContext.IngredientTranslations.Add(new IngredientTranslation()
                {
                    Name = ingredient.EnglishName,
                    NamePlural = ingredient.EnglishNamePlural,
                    Description = ingredient.EnglishDescription,
                    IngredientId = ingredient.Id,
                    LanguageId = 20
                });
            }
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateFilteredIngredientsAsync(IQueryable<Ingredient> ingredients, ICollection<Tag> tagsToAdd, ICollection<Tag> tagsToRemove, int? defaultMeasurementId, int? defaultNutrientMeasurementId)
        {
            foreach (Ingredient ingredient in ingredients)
            {
                ingredient.UpdatedAt = DateTime.UtcNow;
                if (tagsToRemove.Count() > 0)
                {
                    var tags = ingredient.Tags.ToList();
                    tags.RemoveAll(x => tagsToRemove.Any(test => test.Id == x.Id));
                    ingredient.Tags = tags;
                }
                if(tagsToAdd.Count() > 0)
                {
                    var addedTags = ingredient.Tags.Union(tagsToAdd).ToList();
                    ingredient.Tags = addedTags.DistinctBy(x => x.Id).ToList();
                }
                if(defaultMeasurementId != null)
                {
                    ingredient.DefaultMeasurementId = defaultMeasurementId;
                }
                if (defaultNutrientMeasurementId != null)
                {
                    ingredient.DefaultNutrientMeasurementId = defaultNutrientMeasurementId;
                }
                _dbContext.Ingredients.Update(ingredient);
            }
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateIngredientsAsync(List<Ingredient> ingredients)
        {
            foreach(Ingredient ingredient in ingredients)
            {
                var item = await GetIngredientAsync(ingredient.Id)
                    .ConfigureAwait(false);

                // update ingredient
                item.UpdatedAt = DateTime.UtcNow;
                item.EnglishName = ingredient.EnglishName;
                item.EnglishNamePlural = ingredient.EnglishNamePlural;
                item.EnglishDescription = ingredient.EnglishDescription;
                item.DefaultMeasurementId = ingredient.DefaultMeasurementId;
                item.DefaultNutrientMeasurementId = ingredient.DefaultNutrientMeasurementId;
                item.OneCentimeterInGram = ingredient.OneCentimeterInGram;
                item.OneCentimeterInMilliliter = ingredient.OneCentimeterInMilliliter;
                item.OneMilliliterInGram = ingredient.OneMilliliterInGram;
                item.OnePieceInCentimeter = ingredient.OnePieceInCentimeter;
                item.OnePieceInGram = ingredient.OnePieceInGram;
                item.OnePieceInMilliliter = ingredient.OnePieceInMilliliter;
                item.WikidataId = ingredient.WikidataId;
                item.Published = ingredient.Published;

                // update ingredient tags
                var toRemove = item.Tags.ToList().Where(p => !ingredient.Tags.ToList().Any(p2 => p2.Id == p.Id));
                var toAdd = ingredient.Tags.ToList().Where(p => !item.Tags.ToList().Any(p2 => p2.Id == p.Id));
                foreach (var removeTag in toRemove)
                {
                    _dbContext.RawQuery($"DELETE FROM TagToIngredient WHERE TagsId = {removeTag.Id} AND IngredientsId = {ingredient.Id}");
                }
                foreach (var addTag in toAdd)
                {
                    _dbContext.RawQuery($"INSERT INTO TagToIngredient (TagsId, IngredientsId) VALUES ({addTag.Id},  {ingredient.Id})");
                }


                // update ingredient translations
                if (ingredient.Translations != null)
                {
                    var dbTranslations = _dbContext.IngredientTranslations.Where(p => p.IngredientId == ingredient.Id);
                    int translationCounter = 0;
                    foreach(var translation in ingredient.Translations)
                    {
                        var dbTranslation = await dbTranslations.FirstOrDefaultAsync(_ => _.LanguageId == translation.LanguageId);
                        if(dbTranslation != null)
                        {
                            if (String.IsNullOrWhiteSpace(translation.Name) && String.IsNullOrWhiteSpace(translation.Description) && String.IsNullOrWhiteSpace(translation.NamePlural))
                            {
                                var entity = await _dbContext.IngredientTranslations.FindAsync(translation.IngredientId, translation.LanguageId); //To Avoid tracking error
                                _dbContext.Entry(entity).State = EntityState.Deleted;
                            }
                            else
                            {
                                dbTranslation.Name = translation.Name;
                                dbTranslation.NamePlural = translation.NamePlural;
                                dbTranslation.Description = translation.Description;
                                translationCounter++;
                            }
                        }
                        else if(!String.IsNullOrWhiteSpace(translation.Name))
                        {
                            translation.IngredientId = ingredient.Id;
                            _dbContext.IngredientTranslations.Add(translation);
                            translationCounter++;
                        }
                    }
                    item.TranslationsAmount = translationCounter;

                }

                // update ingredient nutrients
                if(ingredient.Nutrients != null)
                {
                    var dbNutrients = _dbContext.IngredientNutrients.Where(p => p.IngredientId == ingredient.Id);
                    int nutrientsCounter = 0;
                    foreach (var nutrient in ingredient.Nutrients)
                    {
                        var dbNutrient = await dbNutrients.FirstOrDefaultAsync(_ => _.NutrientId == nutrient.NutrientId);
                        if (dbNutrient != null)
                        {
                            dbNutrient.Amount = nutrient.Amount;
                            nutrientsCounter++;
                        }
                        else if (!Double.IsNaN(nutrient.Amount))
                        {
                            nutrient.IngredientId = ingredient.Id;
                            _dbContext.IngredientNutrients.Add(nutrient);
                            nutrientsCounter++;
                        }
                    }
                    item.NutrientsAmount = nutrientsCounter;
                }


                // add image
                if (!String.IsNullOrEmpty(item.ImageUrl) && String.IsNullOrEmpty(ingredient.ImageUrl))
                {
                    await _imageRepository.RemoveImageFolderAsync($"{item.Id}", blobContainerName: "ingredient-images");
                    item.ImageUrl = null;
                }
                else if (!String.IsNullOrEmpty(ingredient.ImageUrl) && ingredient.ImageUrl.StartsWith("data"))
                {
                    await _imageRepository.StoreImageAsync(ingredient.ImageUrl, $"{item.Id}", name: "thumbnail", maxWidth: 256, blobContainerName: "ingredient-images");
                    var imagePath = await _imageRepository.StoreImageAsync(ingredient.ImageUrl, $"{item.Id}", name: "main", maxWidth: 1280, blobContainerName: "ingredient-images");
                    item.ImageUrl = imagePath.ReplaceLast("/main", "/");
                }
                else if (!String.IsNullOrEmpty(ingredient.ImageUrl) && ingredient.ImageUrl != $"/ingredient-images/{item.Id}/")
                {
                    throw new InvalidOperationException("Image path is not correct");
                }

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            }
        }
        IQueryable<IngredientTranslation> IIngredientRepository.GetIngredientTranslationsAsync(int? startAt, int? amount, string search)
        {
            int languageId = _headerService.GetLanguageId() ?? 20;
            var amountNumber = amount != null ? amount.GetValueOrDefault() : 50;
            var startAtNumber = startAt != null ? startAt.GetValueOrDefault() : 0;


            IQueryable<IngredientTranslation> query = _dbContext.IngredientTranslations.AsNoTracking()
                .OrderBy(a => a.Name.Length)
                .Where(t => t.LanguageId == languageId);

            if(!String.IsNullOrWhiteSpace(search))
            {
                string[] searchArray = search.ToLower().Trim().Split(" ");
                foreach (string searchWord in searchArray.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    query = query.Where(t => t.Name.ToLower().Contains(searchWord.Trim()) || t.NamePlural.ToLower().Contains(searchWord.Trim()));
                }
            }

            query = query.Include(i => i.Ingredient).Where(_ => _.Ingredient.Archived != true).Skip(startAtNumber).Take(amountNumber);
            return query;
        }
        IQueryable<Ingredient> IIngredientRepository.GetIngredientsByIdAsync(List<int> ids)
        {
            int languageId = _headerService.GetLanguageId() ?? 20;

            IQueryable<Ingredient> query = _dbContext.Ingredients.AsNoTracking()
                .Where(t => ids.Contains(t.Id)).Include(i => i.Translations.Where(t => t.LanguageId == languageId));

            return query;
        }
    }
}
