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
using WorldsBelly.Domain.Utils.Helpers;

namespace WorldsBelly.DataAccess.Repository
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IHeaderService _headerService;

        public MeasurementRepository(AppDbContext dbContext, IHeaderService headerService)
        {
            _dbContext = dbContext;
            _headerService = headerService;
        }

        public async Task<Measurement> CreateMeasurementAsync(Measurement entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            _dbContext.Measurements.Add(entity);
            await _dbContext.SaveChangesAsync();

            return await GetMeasurementAsync(entity.Id);
        }

        public async Task<Measurement> GetMeasurementAsync(int id)
        {
            return await _dbContext.Measurements
                .Include(i => i.Translations)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IQueryable<Measurement>> GetMeasurementsAsync()
        {
            int? languageId = _headerService.GetLanguageId();
            if (languageId == null)
            {
                return _dbContext.Measurements;
            }
            return _dbContext.Measurements.Where(m => !m.IsHidden)
                .Include(m => m.Translations.Where(t => t.LanguageId == languageId));
        }

        public async Task UpdateMeasurementAsync(Measurement measurement)
        {
            var item = await GetMeasurementAsync(measurement.Id)
                .ConfigureAwait(false);

            // update ingredient
            item.UpdatedAt = DateTime.UtcNow;
            item.EnglishName = measurement.EnglishName;
            item.EnglishNamePlural = measurement.EnglishNamePlural;
            item.EnglishDescription = measurement.EnglishDescription;
            item.HasMultipleConversions = measurement.HasMultipleConversions;
            item.IsImperial = measurement.IsImperial;
            item.IsMetric = measurement.IsMetric;
            item.BaseUnitId = measurement.BaseUnitId;
            item.WikidataId = measurement.WikidataId;

            // update ingredient translations
            var dbTranslations = _dbContext.MeasurementTranslations.Where(p => p.MeasurementId == measurement.Id);
            foreach (var translation in measurement.Translations)
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
                    translation.MeasurementId = measurement.Id;
                    _dbContext.MeasurementTranslations.Add(translation);
                }
            }

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }
    }
}
