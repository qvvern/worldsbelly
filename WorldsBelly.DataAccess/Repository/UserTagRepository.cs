//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using WorldsBelly.DataAccess.Contexts;
//using WorldsBelly.DataAccess.Entities;
//using WorldsBelly.DataAccess.Repository.Interfaces;
//using WorldsBelly.DataAccess.Services.Interfaces;
//using WorldsBelly.Domain.Utils.Helpers;

//namespace WorldsBelly.DataAccess.Repository
//{
//	public class UserTagRepository : IUserTagRepository
//    {
//        private readonly AppDbContext _dbContext;
//        private readonly IHeaderService _headerService;

//        public UserTagRepository(AppDbContext dbContext, IHeaderService headerService)
//        {
//            _dbContext = dbContext;
//            _headerService = headerService;
//        }

//        public async Task<ICollection<UserTag>> GetOrCreateUserTagsAsync(ICollection<UserTag> usertags, int? languageId = null)
//        {
//            Guid signedInUserId = _headerService.GetUserId();
//            var tagNames = usertags.Select(c => c.Name.ToLower().Trim()).ToList().Distinct().ToList();

//            List<UserTag> existingTags = await _dbContext.UserTags.AsNoTracking()
//                .Where(t => tagNames.Contains(t.Name.ToLower().Trim()))
//                .ToListAsync();

//            ICollection<UserTag> createdTags = new List<UserTag>();

//            bool updateTags = false;
//            foreach (UserTag userTag in usertags.Where(_ => _.Name != null && _.Name.Trim().Length > 2).Distinct())
//            {
//                userTag.LanguageId = languageId != null ? languageId.GetValueOrDefault() : userTag.LanguageId;
//                if (userTag.LanguageId == 0) throw new Exception("Language id cannot be 0");
//                var foundTag = existingTags.FirstOrDefault(o => o.Name.ToLower().Trim() == userTag.Name.ToLower().Trim() && userTag.LanguageId == o.LanguageId);
//                if (foundTag == null)
//                {
//                    updateTags = true;
//                    userTag.Name = userTag.Name.ToLower().Trim();
//                    _dbContext.UserTags.Add(userTag);
//                    createdTags.Add(userTag);
//                }
//                else
//                {
//                    createdTags.Add(foundTag);
//                }
//            }
//            if(updateTags)
//            {
//                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
//            }
//            return createdTags;
//        }

//        public IQueryable<UserTag> GetUserTagsAsync(int? startAt, int? amount, string search)
//        {
//            var amountNumber = amount != null ? amount.GetValueOrDefault() : 50;
//            var startAtNumber = startAt != null ? startAt.GetValueOrDefault() : 0; 
//            int languageId = _headerService.GetLanguageId().GetValueOrDefault();
//            return _dbContext.UserTags
//                .Where(t => t.LanguageId == languageId)
//                .Where(t => t.Name.ToLower().Contains(search.ToLower()))
//                .Skip(startAtNumber)
//                .Take(amountNumber);

//        }
//    }
//}
