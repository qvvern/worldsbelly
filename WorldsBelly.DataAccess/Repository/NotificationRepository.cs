using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Hubs;
using WorldsBelly.DataAccess.Migrations;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services;
using WorldsBelly.DataAccess.Services.Interfaces;
using WorldsBelly.Domain.Models;
using WorldsBelly.Domain.Utils.Helpers;

namespace WorldsBelly.DataAccess.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IHeaderService _headerService;
        private IUserRepository _userRepos;
        private readonly IHubContext<WorldsBellyHub> _worldsbellyHubContext;

        public NotificationRepository(AppDbContext dbContext, IHeaderService headerService, IUserRepository userRepos, IHubContext<WorldsBellyHub> worldsbellyHubContext)
        {
            _dbContext = dbContext;
            _headerService = headerService;
            _userRepos = userRepos;
            _worldsbellyHubContext = worldsbellyHubContext;
        }

        public async Task CreateNotificationAsync(Notification notification)
        {
            if (notification.SenderId == notification.ReceiverId) return;
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
            await SignalNotificationCreated(notification.ReceiverId, notification);
        }

        private async Task SignalNotificationCreated(int recieverId, Notification message)
        {
            User user = await _userRepos.GetUserByIdAsync(recieverId);
            await _worldsbellyHubContext.Clients
                .Group(user.ADObjectId.ToString())
                .SendCoreAsync("Notification",
                new object[] {
                new
                {
                    Id = message.Id,
                } });
        }

        public async Task<IQueryable<Notification>> GetNotificationsBySignedInUserAsync(int? startAt, int? amount)
        {
            var amountNumber = amount != null ? amount.GetValueOrDefault() : 50;
            var startAtNumber = startAt != null ? startAt.GetValueOrDefault() : 0;
            int languageId = _headerService.GetLanguageId() ?? 20;
            var currentUser = _headerService.GetUserId();
            User user = await _userRepos.GetUserByAzureIdAsync(currentUser);
            if (user == null)
            {
                throw new Exception("Could not find user");
            }
            return _dbContext.Notifications
                .AsNoTracking()
                .OrderBy(_ => _.IsRead == true)
                .Include(i => i.Template)
                    .ThenInclude(i => i.Translations.Where(_ => _.LanguageId == languageId))
                .Include(_ => _.Sender)
                .Where(_ => _.ReceiverId == user.Id)
                .Skip(startAtNumber).Take(amountNumber);
        }

        public async Task<Notification> GetNotificationBySignedInUserAsync(int id)
        {
            int languageId = _headerService.GetLanguageId() ?? 20;
            var currentUser = _headerService.GetUserId();
            User user = await _userRepos.GetUserByAzureIdAsync(currentUser);
            if (user == null)
            {
                throw new Exception("Could not find user");
            }
            return _dbContext.Notifications
                .AsNoTracking()
                .Include(i => i.Template)
                    .ThenInclude(i => i.Translations.Where(_ => _.LanguageId == languageId))
                .Include(_ => _.Sender)
                .Where(_ => _.ReceiverId == user.Id)
                .SingleOrDefault(_ => _.Id == id);
        }

        public async Task DeleteNotificationsBySignedInUserAsync()
        {
            var currentUser = _headerService.GetUserId();
            User user = await _userRepos.GetUserByAzureIdAsync(currentUser);
            if (user == null)
            {
                throw new Exception("Could not find user");
            }
            var notifications = _dbContext.Notifications.Where(_ => _.ReceiverId == user.Id);
            _dbContext.RemoveRange(notifications);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ReadNotificationBySignedInUserAsync(int id)
        {
            int languageId = _headerService.GetLanguageId() ?? 20;
            var currentUser = _headerService.GetUserId();
            User user = await _userRepos.GetUserByAzureIdAsync(currentUser);
            if (user == null)
            {
                throw new Exception("Could not find user");
            }
            var notification = _dbContext.Notifications.Where(_ => _.ReceiverId == user.Id).SingleOrDefault(_ => _.Id == id);
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            _dbContext.Update(notification);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ReadNotificationsBySignedInUserAsync()
        {
            int languageId = _headerService.GetLanguageId() ?? 20;
            var currentUser = _headerService.GetUserId();
            User user = await _userRepos.GetUserByAzureIdAsync(currentUser);
            if (user == null)
            {
                throw new Exception("Could not find user");
            }
            var notifications = _dbContext.Notifications.Where(_ => _.ReceiverId == user.Id);
            foreach(var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
            }
            _dbContext.UpdateRange(notifications);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Notification> GetNotificationAsync(int id)
        {
            int languageId = _headerService.GetLanguageId() ?? 20;
            return await _dbContext.Notifications
                .AsNoTracking()
                .Include(i => i.Template)
                    .ThenInclude(i => i.Translations.Where(_ => _.LanguageId == languageId))
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<NotificationTemplate> CreateNotificationTemplateAsync(NotificationTemplate notificationTemplate)
        {
            notificationTemplate.Translations.Add(new NotificationTemplateTranslation()
            {
                TemplateId = notificationTemplate.Id,
                LanguageId = 20,
                Title = notificationTemplate.EnglishTitle,
                Content = notificationTemplate.EnglishContent,
            });
            _dbContext.NotificationTemplates.Add(notificationTemplate);
            await _dbContext.SaveChangesAsync();

            return await GetNotificationTemplateAsync(notificationTemplate.Id);
        }

        public async Task DeleteNotificationTemplatesAsync(List<int> notificationTemplateIds)
        {
            foreach (var notificationTemplateId in notificationTemplateIds)
            {
                var item = await GetNotificationTemplateAsync(notificationTemplateId).ConfigureAwait(false);

                _dbContext.Remove(item);
            }
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<NotificationTemplate> GetNotificationTemplateAsync(int id)
        {
            return await _dbContext.NotificationTemplates
                .Include(i => i.Translations)
                .Include(_ => _.Type)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IQueryable<NotificationTemplate>> GetNotificationTemplatesAsync(int? languageId = null)
        {
            var items = _dbContext.NotificationTemplates.AsNoTracking();
            if (languageId != null)
            {
                return items.Include(i => i.Translations.Where(_ => _.LanguageId == languageId));
            }
            return items.Include(i => i.Translations).Include(_ => _.Type);
        }

        public async Task UpdateNotificationTemplateAsync(NotificationTemplate notificationTemplate)
        {
            var item = await GetNotificationTemplateAsync(notificationTemplate.Id)
                .ConfigureAwait(false);

            // update ingredient
            item.EnglishTitle = notificationTemplate.EnglishTitle;
            item.EnglishContent = notificationTemplate.EnglishContent;

            // update ingredient translations
            var dbTranslations = _dbContext.NotificationTemplateTranslations.Where(p => p.TemplateId == notificationTemplate.Id);
            foreach (var translation in notificationTemplate.Translations)
            {
                var dbTranslation = await dbTranslations.FirstOrDefaultAsync(_ => _.LanguageId == translation.LanguageId);
                if (dbTranslation != null)
                {
                    dbTranslation.Title = translation.Title;
                    dbTranslation.Content = translation.Content;
                }
                else if (translation.Title != null)
                {
                    translation.TemplateId = notificationTemplate.Id;
                    _dbContext.NotificationTemplateTranslations.Add(translation);
                }
            }

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<NotificationType> CreateNotificationTypeAsync(NotificationType notificationType)
        {
            _dbContext.NotificationTypes.Add(notificationType);
            await _dbContext.SaveChangesAsync();

            return await GetNotificationTypeAsync(notificationType.Id);
        }

        public async Task DeleteNotificationTypesAsync(List<int> notificationTypeIds)
        {
            foreach (var notificationTypeId in notificationTypeIds)
            {
                var item = await GetNotificationTypeAsync(notificationTypeId).ConfigureAwait(false);

                _dbContext.Remove(item);
            }
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<NotificationType> GetNotificationTypeAsync(int id)
        {
            return await _dbContext.NotificationTypes.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IQueryable<NotificationType>> GetNotificationTypesAsync(int? languageId = null)
        {
            return _dbContext.NotificationTypes.AsNoTracking();
        }

        public async Task UpdateNotificationTypeAsync(NotificationType notificationType)
        {
            var item = await GetNotificationTypeAsync(notificationType.Id)
                .ConfigureAwait(false);

            item.Name = notificationType.Name;

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
