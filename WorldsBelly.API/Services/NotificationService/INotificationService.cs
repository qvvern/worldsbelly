using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldsBelly.API.Models;

namespace WorldsBelly.API.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ActionResult<ICollection<NotificationView>>> GetNotificationsAsync(int? startAt, int? amount);
        Task<ActionResult<NotificationView>> GetNotificationAsync(int id);
        Task DeleteNotificationsAsync();
        Task ReadNotificationAsync(int id);
        Task ReadNotificationsAsync();

    }
}
