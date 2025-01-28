using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
    public interface INotificationRepository
    {
        // notifications
        Task CreateNotificationAsync(Notification notification);
        Task<IQueryable<Notification>> GetNotificationsBySignedInUserAsync(int? startAt, int? amount);
        Task<Notification> GetNotificationAsync(int id);
        Task<Notification> GetNotificationBySignedInUserAsync(int id);
        Task DeleteNotificationsBySignedInUserAsync();
        Task ReadNotificationBySignedInUserAsync(int id);
        Task ReadNotificationsBySignedInUserAsync();

        // templates
        Task<IQueryable<NotificationTemplate>> GetNotificationTemplatesAsync(int? languageId = null);
        Task<NotificationTemplate> GetNotificationTemplateAsync(int id);
        Task DeleteNotificationTemplatesAsync(List<int> notificationTemplates);
        Task<NotificationTemplate> CreateNotificationTemplateAsync(NotificationTemplate notificationTemplate);
        Task UpdateNotificationTemplateAsync(NotificationTemplate notificationTemplate);

        // types
        Task<IQueryable<NotificationType>> GetNotificationTypesAsync(int? languageId = null);
        Task<NotificationType> GetNotificationTypeAsync(int id);
        Task DeleteNotificationTypesAsync(List<int> notificationTypes);
        Task<NotificationType> CreateNotificationTypeAsync(NotificationType notificationType);
        Task UpdateNotificationTypeAsync(NotificationType notificationType);
    }
}
