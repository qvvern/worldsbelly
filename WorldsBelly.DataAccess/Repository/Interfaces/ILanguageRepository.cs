using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
    public interface ILanguageRepository
    {
        Task<IQueryable<Language>> GetLanguagesAsync();
        Task<Language> GetLanguageAsync(int id);
        Task UpdateLanguageAsync(Language language);
    }
}
