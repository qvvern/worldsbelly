using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Entities.Filters;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
	public interface ITagSelectableRepository
	{
		// 1
		Task<TagSelectable> GetTagSelectableAsync(int id);
		Task<TagSelectable> CreateTagSelectableAsync(TagSelectable entity);
		Task UpdateTagSelectableAsync(TagSelectable tag);
		Task DeleteTagSelectablesAsync(List<int> tagIds);
		IQueryable<TagSelectable> GetTagSelectablesAsync();
		// 2
		Task<TagSelectableCategory> CreateTagSelectableCategoryAsync(TagSelectableCategory entity);
		Task<TagSelectableCategory> GetTagSelectableCategoryAsync(int id);
		Task DeleteTagSelectableCategoriesAsync(List<int> ids);
		Task UpdateTagSelectableCategoryAsync(TagSelectableCategory tag);
		IQueryable<TagSelectableCategory> GetTagSelectableCategoriesAsync();
		// 3
		Task<TagSelectableChoiceOrder> CreateTagSelectableChoiceOrderAsync(TagSelectableChoiceOrder entity);
		Task<TagSelectableChoiceOrder> GetTagSelectableChoiceOrderAsync(int id);
		Task DeleteTagSelectableChoiceOrdersAsync(List<int> tagIds);
		Task UpdateTagSelectableChoiceOrderAsync(TagSelectableChoiceOrder tag);
		IQueryable<TagSelectableChoiceOrder> GetTagSelectableChoiceOrdersAsync();

	}
}
