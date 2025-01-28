using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Entities.Filters;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
	public interface ITranslationRepository
	{
		IQueryable<EnglishTranslation> GetTranslationsQuery(SortingOptions sortingOptions);
		Task<EnglishTranslation> GetTranslationAsync(int id);
		Task<EnglishTranslation> CreateTranslationAsync(EnglishTranslation entity);
		Task UpdateTranslationsAsync(List<EnglishTranslation> translations);
		Task DeleteTranslationsAsync(List<int> translationIds);
		IQueryable<EnglishTranslation> GetTranslationsBasicAsync(int? limit, string search);
		Task<ICollection<EnglishTranslation>> GetTranslationsByIdsAsync(List<int> ids);

	}
}
