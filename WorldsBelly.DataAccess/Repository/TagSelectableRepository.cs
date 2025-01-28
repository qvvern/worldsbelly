using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Entities.Filters;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;
using WorldsBelly.Domain.Utils.Helpers;

namespace WorldsBelly.DataAccess.Repository
{
    public class TagSelectableRepository : ITagSelectableRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IHeaderService _headerService;

        public TagSelectableRepository(AppDbContext dbContext, IHeaderService headerService)
        {
            _dbContext = dbContext;
            _headerService = headerService;
        }

        public async Task<TagSelectable> CreateTagSelectableAsync(TagSelectable entity)
        {
            _dbContext.TagSelectables.Add(entity);
            await _dbContext.SaveChangesAsync();

            return await GetTagSelectableAsync(entity.Id);
        }

        public async Task<TagSelectable> GetTagSelectableAsync(int id)
        {
            return _dbContext.TagSelectables
                .Where(_ => _.Id == id)
                //.Include(t => t.Tag)
                //.Include(t => t.Categories)
                .FirstOrDefault();
        }

        public async Task DeleteTagSelectablesAsync(List<int> tagIds)
        {
            foreach (var tagId in tagIds)
            {
                var item = await GetTagSelectableAsync(tagId).ConfigureAwait(false);
                _dbContext.TagSelectables.Remove(item);
            }
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        public async Task UpdateTagSelectableAsync(TagSelectable tag)
        {
            var item = await GetTagSelectableAsync(tag.Id).ConfigureAwait(false);
            item.TagId = tag.TagId;
            item.Icon = tag.Icon;
            item.ExcludedTags = tag.ExcludedTags;
            item.OrderIndex = tag.OrderIndex;
            item.DontAddTag = tag.DontAddTag;

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public IQueryable<TagSelectable> GetTagSelectablesAsync()
        {
            int? languageId = _headerService.GetLanguageId();
            var items = _dbContext.TagSelectables.Include(i => i.Tag);
            if (languageId != null)
            {
                return items.ThenInclude(t => t.Translations.Where(_ => _.LanguageId == languageId));
            }
            return items; 
        }

        /* 
         * TagSelectableCategory
         * */


        public async Task<TagSelectableCategory> CreateTagSelectableCategoryAsync(TagSelectableCategory entity)
        {
            _dbContext.TagSelectableCategories.Add(entity);
            await _dbContext.SaveChangesAsync();

            return await GetTagSelectableCategoryAsync(entity.Id);
        }

        public async Task<TagSelectableCategory> GetTagSelectableCategoryAsync(int id)
        {
            return _dbContext.TagSelectableCategories
                .Where(_ => _.Id == id)
                .Include(_ => _.Text)
                .Include(_ => _.Title)
                .Include(_ => _.TagSelectables)
                    .ThenInclude(t => t.Tag)
                .FirstOrDefault();
        }

        public async Task DeleteTagSelectableCategoriesAsync(List<int> ids)
        {
            foreach (int id in ids)
            {
                var item = await GetTagSelectableCategoryAsync(id).ConfigureAwait(false);
                _dbContext.TagSelectableCategories.Remove(item);
            }
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        public async Task UpdateTagSelectableCategoryAsync(TagSelectableCategory tag)
        {
            var item = await GetTagSelectableCategoryAsync(tag.Id).ConfigureAwait(false);
            item.ExcludedTags = tag.ExcludedTags;
            item.TextId = tag.TextId;
            item.TitleId = tag.TitleId;

            // Remove
            foreach (TagSelectable tagSelectable in item.TagSelectables)
            {
                var foundTag = tag.TagSelectables.FirstOrDefault(_ => _.Id == tagSelectable.Id);
                if (foundTag == null)
                {
                    _dbContext.Entry(tagSelectable).State = EntityState.Deleted;
                }
            }
            // Add
            List<TagSelectable> selectablesToAdd = tag.TagSelectables.Where(p => item.TagSelectables.All(p2 => p2.Id != p.Id)).ToList();
            foreach (TagSelectable tagSelectable in selectablesToAdd)
            {
                item.TagSelectables.Add(tagSelectable);
            }

            _dbContext.TagSelectableCategories.Update(item);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public IQueryable<TagSelectableCategory> GetTagSelectableCategoriesAsync()
        {
            int? languageId = _headerService.GetLanguageId();
            var items = _dbContext.TagSelectableCategories
                .Include(_ => _.Text)
                .Include(_ => _.Title)
                .Include(_ => _.TagSelectables)
                    .ThenInclude(t => t.Tag);
            if (languageId != null)
            {
                return items.Include(t => t.Text.Translations.Where(_ => _.LanguageId == languageId))
                    .Include(t => t.Title.Translations.Where(_ => _.LanguageId == languageId))
                    .Include(t => t.TagSelectables)
                        .ThenInclude(t => t.Tag.Translations.Where(_ => _.LanguageId == languageId));
            }
            return items;
        }

        /* 
         * TagSelectableChoiceOrder
         * */



        public async Task<TagSelectableChoiceOrder> CreateTagSelectableChoiceOrderAsync(TagSelectableChoiceOrder entity)
        {
            _dbContext.TagSelectableChoiceOrders.Add(entity);
            await _dbContext.SaveChangesAsync();

            return await GetTagSelectableChoiceOrderAsync(entity.Id);
        }

        public async Task<TagSelectableChoiceOrder> GetTagSelectableChoiceOrderAsync(int id)
        {
            return _dbContext.TagSelectableChoiceOrders
                .Where(_ => _.Id == id)
                .Include(_ => _.TagSelectableCategory)
                    .ThenInclude(t => t.Text)
                .FirstOrDefault();
        }

        public async Task DeleteTagSelectableChoiceOrdersAsync(List<int> tagIds)
        {
            foreach (var tagId in tagIds)
            {
                var item = await GetTagSelectableChoiceOrderAsync(tagId).ConfigureAwait(false);
                _dbContext.TagSelectableChoiceOrders.Remove(item);
            }
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        public async Task UpdateTagSelectableChoiceOrderAsync(TagSelectableChoiceOrder tag)
        {
            var item = await GetTagSelectableChoiceOrderAsync(tag.Id).ConfigureAwait(false);
            item.TagId = tag.TagId;
            item.RelatedChoiceId = tag.RelatedChoiceId;
            item.OrderIndex = tag.OrderIndex;
            item.TagSelectableCategoryId = tag.TagSelectableCategoryId;

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public IQueryable<TagSelectableChoiceOrder> GetTagSelectableChoiceOrdersAsync()
        {
            int? languageId = _headerService.GetLanguageId();
            var items = _dbContext.TagSelectableChoiceOrders
                .Include(_ => _.TagSelectableCategory)
                    .ThenInclude(t => t.Text)
                .Include(_ => _.TagSelectableCategory)
                    .ThenInclude(t => t.Title)
                .Include(_ => _.TagSelectableCategory)
                    .ThenInclude(t => t.TagSelectables)
                        .ThenInclude(t => t.Tag);

            if (languageId != null)
            {
                return items
                .Include(_ => _.TagSelectableCategory)
                    .ThenInclude(t => t.Title)
                        .ThenInclude(t => t.Translations.Where(_ => _.LanguageId == languageId))
                .Include(_ => _.TagSelectableCategory)
                    .ThenInclude(t => t.Text)
                        .ThenInclude(t => t.Translations.Where(_ => _.LanguageId == languageId))
                .Include(_ => _.TagSelectableCategory)
                    .ThenInclude(t => t.TagSelectables)
                        .ThenInclude(t => t.Tag)
                            .ThenInclude(t => t.Translations.Where(_ => _.LanguageId == languageId));
            };

            return items;
        }

    }
}
