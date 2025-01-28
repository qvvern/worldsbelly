using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Entities.Filters;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;

namespace WorldsBelly.DataAccess.Repository
{
    public class TranslationRepository : ITranslationRepository
    {
        private readonly AppDbContext _dbContext;

        public TranslationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EnglishTranslation> CreateTranslationAsync(EnglishTranslation entity)
        {

            entity.CreatedAt = DateTime.UtcNow;
            _dbContext.EnglishTranslations.Add(entity);
            await _dbContext.SaveChangesAsync();


            Translation englishTranslation = new Translation()
            {
                EnglishTranslationId = entity.Id,
                LanguageId = 20,
                Text = entity.Text,
                TextPlural = entity.TextPlural,
            };

            _dbContext.Translations.Add(englishTranslation);
            await _dbContext.SaveChangesAsync();

            return await GetTranslationAsync(entity.Id);
        }

        public async Task<EnglishTranslation> GetTranslationAsync(int id)
        {
            return _dbContext.EnglishTranslations
                .Where(_ => _.Id == id)
                //.Include(i => i.Ingredients)
                .Include(i => i.Translations)
                .FirstOrDefault();
        }

        public IQueryable<EnglishTranslation> GetTranslationsQuery(SortingOptions sortingOptions)
        {
            return _dbContext.EnglishTranslations
                //.Include(i => i.Ingredients)
                .Include(i => i.Translations)
                .ApplySorting(sortingOptions);
        }

        public async Task DeleteTranslationsAsync(List<int> translationIds)
        {
            foreach (var translationId in translationIds)
            {
                var item = await GetTranslationAsync(translationId).ConfigureAwait(false);
                _dbContext.Remove(item);
            }
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        public async Task UpdateTranslationsAsync(List<EnglishTranslation> translations)
        {
            foreach (EnglishTranslation englishTranslation in translations)
            {
                var item = await GetTranslationAsync(englishTranslation.Id)
                    .ConfigureAwait(false);

                // update translation
                item.UpdatedAt = DateTime.UtcNow;
                item.Text = englishTranslation.Text;
                item.TextPlural = englishTranslation.TextPlural;

                // update translations
                var dbTranslations = _dbContext.Translations.Where(p => p.EnglishTranslationId == englishTranslation.Id);
                int translationCounter = 0;
                foreach (var translation in englishTranslation.Translations)
                {
                    var dbTranslation = await dbTranslations.FirstOrDefaultAsync(_ => _.LanguageId == translation.LanguageId);
                    if (dbTranslation != null)
                    {
                        if (String.IsNullOrWhiteSpace(translation.Text) && String.IsNullOrWhiteSpace(translation.TextPlural))
                        {
                            var entity = await _dbContext.Translations.FindAsync(translation.EnglishTranslationId, translation.EnglishTranslationId); //To Avoid tracking error
                            _dbContext.Entry(entity).State = EntityState.Deleted;
                        }
                        else
                        {
                            dbTranslation.Text = translation.Text;
                            dbTranslation.TextPlural = translation.TextPlural;
                        }
                    }
                    else if (!String.IsNullOrWhiteSpace(translation.Text))
                    {
                        translation.EnglishTranslationId = englishTranslation.Id;
                        _dbContext.Translations.Add(translation);
                        translationCounter++;
                    }
                }
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<ActionResult<List<EnglishTranslation>>> GetTranslationsBasicAsync()
        {
            return await _dbContext.EnglishTranslations
                .Select(x => new EnglishTranslation
                {
                    Text = x.Text,
                    Id = x.Id,
                }).ToListAsync();
        }

        public IQueryable<EnglishTranslation> GetTranslationsBasicAsync(int? limit, string search)
        {
            if (limit != null && !String.IsNullOrEmpty(search))
            {
                return _dbContext.EnglishTranslations.Where(x => x.Text.ToLower().Contains(search.ToLower())).OrderByDescending(x => x.Text).Take(limit.Value);
            }
            else if (limit == null && !String.IsNullOrEmpty(search))
            {
                return _dbContext.EnglishTranslations.Where(x => x.Text.ToLower().Contains(search.ToLower()));
            }
            else if (limit != null && String.IsNullOrEmpty(search))
            {
                return _dbContext.EnglishTranslations.OrderByDescending(x => x.Text).Take(limit.Value);
            }
            else
            {
                return _dbContext.EnglishTranslations;
            }
        }

        public async Task<ICollection<EnglishTranslation>> GetTranslationsByIdsAsync(List<int> ids)
        {
            return await _dbContext.EnglishTranslations.Where(t => ids.Contains(t.Id)).ToListAsync();
        }
    }
}
