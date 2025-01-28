using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
    public interface ICountryRepository
    {
        // Portal
        Task<IQueryable<Country>> GetCountriesAsync(int? languageId = null);
        Task<Country> GetCountryAsync(int id);
        Task UpdateCountryAsync(Country country);
        Task<Country> CreateCountryAsync(Country entity);
    }
}
