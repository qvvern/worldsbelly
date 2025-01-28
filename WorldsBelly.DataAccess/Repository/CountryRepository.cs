using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.Domain.Utils.Helpers;

namespace WorldsBelly.DataAccess.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _dbContext;

        public CountryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Country> CreateCountryAsync(Country entity)
        {
            _dbContext.Countries.Add(entity);
            await _dbContext.SaveChangesAsync();

            return await GetCountryAsync(entity.Id);
        }

        public async Task<Country> GetCountryAsync(int id)
        {
            return await _dbContext.Countries
                .Include(i => i.Translations)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IQueryable<Country>> GetCountriesAsync(int? languageId = null)
        {
            var items = _dbContext.Countries.AsNoTracking();
            if (languageId != null)
            {
                return items.Include(i => i.Translations.Where(_ => _.LanguageId == languageId));
            }
            return items.Include(i => i.Translations);
        }

        public async Task UpdateCountryAsync(Country country)
        {
            var item = await GetCountryAsync(country.Id)
                .ConfigureAwait(false);

            // update ingredient
            item.EnglishName = country.EnglishName;

            // update ingredient translations
            var dbTranslations = _dbContext.CountryTranslations.Where(p => p.CountryId == country.Id);
            foreach (var translation in country.Translations)
            {
                var dbTranslation = await dbTranslations.FirstOrDefaultAsync(_ => _.LanguageId == translation.LanguageId);
                if (dbTranslation != null)
                {
                    dbTranslation.Name = translation.Name;
                }
                else if (translation.Name != null)
                {
                    translation.CountryId = country.Id;
                    _dbContext.CountryTranslations.Add(translation);
                }
            }

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }
    }
}
