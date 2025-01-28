using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Entities.Filters;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
	public interface ITagRepository
	{
		IQueryable<Tag> GetTagsQuery(SortingOptions sortingOptions);
		Task<Tag> GetTagAsync(int id);
		Task<Tag> CreateTagAsync(Tag entity);
		Task UpdateTagsAsync(List<Tag> tags);
		Task DeleteTagsAsync(List<int> tagIds);
        Task RemoveDuplicatedTagsAsync();
        IQueryable<Tag> GetTagsBasicAsync(int? limit, string search);
		Task<ICollection<Tag>> GetTagsByIdsAsync(List<int> ids);

	}
}
