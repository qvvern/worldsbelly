using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.API.Models;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.API.Utilities.Mappers;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Utilities.Extensions;

namespace WorldsBelly.API.Services
{
    public class NotificationService : INotificationService
    {
        private INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<ActionResult<ICollection<NotificationView>>> GetNotificationsAsync(int? startAt, int? amount)
        {
            var response = await _notificationRepository.GetNotificationsBySignedInUserAsync(startAt, amount);
            return response.Select(ResponseMapper.Map).ToList();
        }

        public async Task<ActionResult<NotificationView>> GetNotificationAsync(int id)
        {
            var response = await _notificationRepository.GetNotificationBySignedInUserAsync(id);
            return ResponseMapper.Map(response);
        }

        public async Task DeleteNotificationsAsync()
        {
            await _notificationRepository.DeleteNotificationsBySignedInUserAsync();
        }

        public async Task ReadNotificationAsync(int id)
        {
            await _notificationRepository.ReadNotificationBySignedInUserAsync(id);
        }
        public async Task ReadNotificationsAsync()
        {
            await _notificationRepository.ReadNotificationsBySignedInUserAsync();
        }
    }
}
