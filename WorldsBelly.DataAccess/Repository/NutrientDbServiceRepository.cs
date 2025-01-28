using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities.NutrientDb;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;

namespace WorldsBelly.DataAccess.Repository
{
    public class NutrientDbServiceRepository : INutrientDbServiceRepository
    {
        private readonly NutrientDbContext _dbContext;
        private readonly AppDbContext _appContext;

        public NutrientDbServiceRepository(NutrientDbContext dbContext, AppDbContext appContext)
        {
            _dbContext = dbContext;
            _appContext = appContext;
        }

        public IQueryable<FoodNutrient> GetFoodNutrientsAsync(int id)
        {
            return _dbContext.food_nutrient.Where(_ => _.Fdc_id == id);
        }
        public IQueryable<Nutrient> GetNutrientsAsync()
        {
           return _dbContext.nutrient;
        }
        public async Task<FoodNutrientConversionFactor> GetFoodNutrientConversionFactorAsync(int id)
        {
            return await _dbContext.food_nutrient_conversion_factor.FirstOrDefaultAsync(_ => _.fdc_id == id);
        }
        public async Task<FoodCalorieConversionFactor> GetFoodCalorieConversionFactorAsync(int id)
        {
            return await _dbContext.food_calorie_conversion_factor.FirstOrDefaultAsync(_ => _.Food_nutrient_conversion_factor_id == id);
        }
        public async Task<FoodProteinConversionFactor> GetFoodProteinConversionFactorAsync(int id)
        {
            return await _dbContext.food_protein_conversion_factor.FirstOrDefaultAsync(_ => _.Food_nutrient_conversion_factor_id == id);
        }

        public IQueryable<Food> GetFoodsAsync(int? limit, string search, int? idSearch)
        {
            if(idSearch != null) 
            {
                return _dbContext.food.Where(x => x.Fdc_id == idSearch);
            }
            if (limit != null && !String.IsNullOrEmpty(search))
            {
                return _dbContext.food.Where(x => x.Description.ToLower().Contains(search.ToLower())).OrderByDescending(x => x.Description).Take(limit.Value);
            }
            else if (limit == null && !String.IsNullOrEmpty(search))
            {
                return _dbContext.food.Where(x => x.Description.ToLower().Contains(search.ToLower()));
            }
            else if (limit != null && String.IsNullOrEmpty(search))
            {
                return _dbContext.food.OrderByDescending(x => x.Description).Take(limit.Value);
            }
            else
            {
                return _dbContext.food;
            }
        }
    }
}
