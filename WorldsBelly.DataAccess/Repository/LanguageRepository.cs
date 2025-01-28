using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository.Interfaces;

namespace WorldsBelly.DataAccess.Repository
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly AppDbContext _dbContext;

        public LanguageRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Language> GetLanguageAsync(int id)
        {
            return await _dbContext.Languages.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IQueryable<Language>> GetLanguagesAsync()
        {
            return _dbContext.Languages.Where(l => !l.IsHidden);
        }

        public async Task UpdateLanguageAsync(Language language)
        {
            language.UpdatedAt = DateTime.UtcNow;
            _dbContext.Languages.Update(language);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
