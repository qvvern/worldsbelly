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
using WorldsBelly.DataAccess.Utilities.Extensions;

namespace WorldsBelly.DataAccess.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _dbContext;

        public TagRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tag> CreateTagAsync(Tag entity)
        {

            entity.CreatedAt = DateTime.UtcNow;
            entity.TranslationsAmount = 1;
            _dbContext.Tags.Add(entity);
            await _dbContext.SaveChangesAsync();


            TagTranslation englishTranslation = new TagTranslation()
            {
                TagId = entity.Id,
                LanguageId = 20,
                Name = entity.EnglishName,
                NamePlural = entity.EnglishNamePlural,
                Description = entity.EnglishDescription
            };

            _dbContext.TagTranslations.Add(englishTranslation);
            await _dbContext.SaveChangesAsync();

            return await GetTagAsync(entity.Id);
        }

        public async Task<Tag> GetTagAsync(int id)
        {
            return _dbContext.Tags
                .Where(_ => _.Id == id)
                //.Include(i => i.Ingredients)
                .Include(i => i.Translations)
                .FirstOrDefault();
        }

        public IQueryable<Tag> GetTagsQuery(SortingOptions sortingOptions)
        {
            return _dbContext.Tags
                .OrderBy(a => a.EnglishName.Length)
                //.Include(i => i.Ingredients)
                .Include(i => i.Translations)
                .ApplySorting(sortingOptions);
        }

        public async Task DeleteTagsAsync(List<int> tagIds)
        {
            foreach (var tagId in tagIds)
            {
                var item = await GetTagAsync(tagId).ConfigureAwait(false);
                _dbContext.Remove(item);
            }
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        public async Task RemoveDuplicatedTagsAsync()
        {
            var tags = await _dbContext.Tags.OrderBy(a => a.EnglishName.Length).ToListAsync();
            var removedTags = new List<Tag>();
            foreach(var tag in tags.ToList())
            {
                if(removedTags.FirstOrDefault(_ => _.Id == tag.Id) != null)
                {
                    continue;
                }
                var duplicatedTags = tags.Where(_ => _.EnglishName.Trim().ToLower() == tag.EnglishName.Trim().ToLower() && _.Id != tag.Id);
                if(duplicatedTags.Count() > 0)
                {
                    foreach(var duplicatedTag in duplicatedTags)
                    {
                        //tags.Remove(duplicatedTag);
                        var ingredientsWithWrongTag = await _dbContext.Ingredients.Include(_ => _.Tags).Where(_ => _.Tags.Any(t => t.Id == duplicatedTag.Id)).ToListAsync();
                        foreach(var ingredientWithWrongTag in ingredientsWithWrongTag)
                        {
                            if (!ingredientWithWrongTag.Tags.Any(_ => _.Id == tag.Id))
                            {
                                ingredientWithWrongTag.Tags.Add(tag);
                            }
                            ingredientWithWrongTag.Tags.Remove(duplicatedTag);
                        }
                    }
                    removedTags.AddRange(duplicatedTags);
                    _dbContext.RemoveRange(duplicatedTags);
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task UpdateTagsAsync(List<Tag> tags)
        {
            foreach (Tag tag in tags)
            {
                var item = await GetTagAsync(tag.Id)
                    .ConfigureAwait(false);

                // update tag
                item.UpdatedAt = DateTime.UtcNow;
                item.EnglishName = tag.EnglishName;
                item.EnglishNamePlural = tag.EnglishNamePlural;
                item.EnglishDescription = tag.EnglishDescription;
                item.WikidataId = tag.WikidataId;
                item.Published = tag.Published;
                item.IncludeAlways = tag.IncludeAlways;
                item.ExcludeAlways = tag.ExcludeAlways;

                // update translations
                var dbTranslations = _dbContext.TagTranslations.Where(p => p.TagId == tag.Id);
                int translationCounter = 0;
                foreach (var translation in tag.Translations)
                {
                    var dbTranslation = await dbTranslations.FirstOrDefaultAsync(_ => _.LanguageId == translation.LanguageId);
                    if (dbTranslation != null)
                    {
                        if (String.IsNullOrWhiteSpace(translation.Name) && String.IsNullOrWhiteSpace(translation.Description) && String.IsNullOrWhiteSpace(translation.NamePlural))
                        {
                            var entity = await _dbContext.TagTranslations.FindAsync(translation.TagId, translation.TagId); //To Avoid tracking error
                            _dbContext.Entry(entity).State = EntityState.Deleted;
                        }
                        else
                        {
                            dbTranslation.Name = translation.Name;
                            dbTranslation.NamePlural = translation.NamePlural;
                            dbTranslation.Description = translation.Description;
                            translationCounter++;
                        }
                    }
                    else if (!String.IsNullOrWhiteSpace(translation.Name))
                    {
                        translation.TagId = tag.Id;
                        _dbContext.TagTranslations.Add(translation);
                        translationCounter++;
                    }
                }
                item.TranslationsAmount = translationCounter;
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            }
        }

        public async Task<ActionResult<List<Tag>>> GetTagsBasicAsync()
        {
            return await _dbContext.Tags
                .Select(x => new Tag
                {
                    EnglishName = x.EnglishName,
                    Id = x.Id,
                }).ToListAsync();
        }

        public IQueryable<Tag> GetTagsBasicAsync(int? limit, string search)
        {
            if (limit != null && !String.IsNullOrEmpty(search))
            {
                return _dbContext.Tags.Where(x => x.EnglishName.ToLower().Contains(search.ToLower())).OrderByDescending(x => x.EnglishName).Take(limit.Value);
            }
            else if (limit == null && !String.IsNullOrEmpty(search))
            {
                return _dbContext.Tags.Where(x => x.EnglishName.ToLower().Contains(search.ToLower()));
            }
            else if (limit != null && String.IsNullOrEmpty(search))
            {
                return _dbContext.Tags.OrderByDescending(x => x.EnglishName).Take(limit.Value);
            }
            else
            {
                return _dbContext.Tags;
            }
        }

        public async Task<ICollection<Tag>> GetTagsByIdsAsync(List<int> ids)
        {
            return await _dbContext.Tags.Where(t => ids.Contains(t.Id)).ToListAsync();
        }
    }
}
