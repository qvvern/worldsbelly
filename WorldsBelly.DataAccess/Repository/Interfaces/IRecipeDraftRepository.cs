using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Entities.Filters;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
	public interface IRecipeDraftRepository
	{
		Task<Recipe> CreateRecipeDraftAsync(Recipe entity);
		Task<Recipe> GetRecipeDraftTranslationAsync(Guid id);
		Task UpdateRecipeDraftAsync(Recipe entity, int languageId);
		Task PublishRecipeDraftAsync(Guid id);
        Task ApproveRecipeDraftAsync(Guid id, bool checkIngredients);
		Task<List<Recipe>> GetRecipeDraftTranslationsAsync();
		Task<ActionResult> DeleteRecipeAsync(int id);
	}
}
