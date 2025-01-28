using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities.NutrientDb;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
	public interface INutrientDbServiceRepository
	{
		IQueryable<Food> GetFoodsAsync(int? limit, string search, int? idSearch);
		IQueryable<FoodNutrient> GetFoodNutrientsAsync(int id);
		IQueryable<Nutrient> GetNutrientsAsync();
		Task<FoodNutrientConversionFactor> GetFoodNutrientConversionFactorAsync(int id);
		Task<FoodCalorieConversionFactor> GetFoodCalorieConversionFactorAsync(int id);
		Task<FoodProteinConversionFactor> GetFoodProteinConversionFactorAsync(int id);

	}
}
