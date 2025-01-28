using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Entities.Filters;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
	public interface IIngredientRepository
	{
		IQueryable<Ingredient> GetIngredientsQuery(SortingOptions sortingOptions);
		IQueryable<Ingredient> GetIngredientsWithTags();
		Task<Ingredient> GetIngredientAsync(int id);
        Task<Ingredient> CreateIngredientAsync(Ingredient entity);
		Task UpdateIngredientsAsync(List<Ingredient> ingredients);
		Task UpdateFilteredIngredientsAsync(IQueryable<Ingredient> ingredients, ICollection<Tag> tagsToAdd, ICollection<Tag> tagsToRemove, int? defaultMeasurementId, int? defaultNutrientMeasurementId);
		Task DeleteIngredientsAsync(List<int> ingredientIds);
		Task ArchiveIngredientsAsync(List<int> ingredientIds);
		Task UpdateIngredientsEnglishtranslations();
        Task RemoveDuplicatedIngredientsAsync();
        IQueryable<IngredientTranslation> GetIngredientTranslationsAsync(int? startAt, int? amount, string search);
        IQueryable<Ingredient> GetIngredientsByIdAsync(List<int> ids);
    }
}
