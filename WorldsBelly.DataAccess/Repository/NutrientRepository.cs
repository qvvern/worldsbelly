using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services.Interfaces;

namespace WorldsBelly.DataAccess.Repository
{
    public class NutrientRepository : INutrientRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IHeaderService _headerService;

        public NutrientRepository(AppDbContext dbContext, IHeaderService headerService)
        {
            _dbContext = dbContext;
            _headerService = headerService;
        }

        public async Task<Nutrient> CreateNutrientAsync(Nutrient entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            _dbContext.Nutrients.Add(entity);
            await _dbContext.SaveChangesAsync();

            return await GetNutrientAsync(entity.Id);
        }

        public async Task<Nutrient> GetNutrientAsync(int id)
        {
            return await _dbContext.Nutrients
                .Include(i => i.Translations)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IQueryable<Nutrient>> GetNutrientsAsync()
        {
            int? languageId = _headerService.GetLanguageId();
            if (languageId == null)
            {
                return _dbContext.Nutrients.OrderBy(_ => _.EnglishName);
            }
            return _dbContext.Nutrients
                .Where(n => n.IsCommon == true)
                .Include(m => m.Translations.Where(t => t.LanguageId == languageId)).OrderBy(_ => _.EnglishName);
        }

        public async Task UpdateNutrientAsync(Nutrient nutrient)
        {
            var item = await GetNutrientAsync(nutrient.Id)
                .ConfigureAwait(false);

            // update ingredient
            item.UpdatedAt = DateTime.UtcNow;
            item.EnglishName = nutrient.EnglishName;
            item.EnglishNamePlural = nutrient.EnglishNamePlural;
            item.EnglishDescription = nutrient.EnglishDescription;
            item.DefaultMeasurementId = nutrient.DefaultMeasurementId;
            item.WikidataId = nutrient.WikidataId;

            // update ingredient translations
            var dbTranslations = _dbContext.NutrientTranslations.Where(p => p.NutrientId == nutrient.Id);
            foreach (var translation in nutrient.Translations)
            {
                var dbTranslation = await dbTranslations.FirstOrDefaultAsync(_ => _.LanguageId == translation.LanguageId);
                if (dbTranslation != null)
                {
                    dbTranslation.Name = translation.Name;
                    dbTranslation.NamePlural = translation.NamePlural;
                    dbTranslation.Description = translation.Description;
                }
                else if (translation.Name != null && translation.NamePlural != null)
                {
                    translation.NutrientId = nutrient.Id;
                    _dbContext.NutrientTranslations.Add(translation);
                }
            }

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }
    }
}
