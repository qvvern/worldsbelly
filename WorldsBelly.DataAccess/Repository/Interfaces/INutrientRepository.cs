using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
    public interface INutrientRepository
    {
        Task<IQueryable<Nutrient>> GetNutrientsAsync();
        Task<Nutrient> GetNutrientAsync(int id);
        Task UpdateNutrientAsync(Nutrient nutrient);
        Task<Nutrient> CreateNutrientAsync(Nutrient entity);
    }
}
