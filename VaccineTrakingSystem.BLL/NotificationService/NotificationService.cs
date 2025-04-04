using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories.NotificationRepository;

namespace VaccineTrakingSystem.BLL.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<int> CreateNotificationAsync(int userId, int? childId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                ChildId = childId,
                Message = message,
                NotificationDate = DateTime.Now,
                IsSent = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return await _notificationRepository.InsertNotificationAsync(notification);
        }

        public Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId)
        {
            return _notificationRepository.GetNotificationsByUserAsync(userId);
        }
    }
}
