using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.ServicesService
{
    public class NotificationService : INotificationService
    {
        private readonly IGenericRepository<Notification> _repository;

        public NotificationService(IGenericRepository<Notification> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _repository.GetAllAsync();
            return notifications.Where(n => n.UserId == userId);
        }

        public async Task<Notification?> GetNotificationByIdAsync(int notificationId)
        {
            return await _repository.GetByIdAsync(notificationId);
        }

        public async Task<int> CreateNotificationAsync(Notification notification)
        {
            notification.CreatedAt = DateTime.UtcNow;
            notification.UpdatedAt = DateTime.UtcNow;
            return await _repository.InsertAsync(notification);
        }

        public async Task<bool> UpdateNotificationAsync(Notification notification)
        {
            notification.UpdatedAt = DateTime.UtcNow;
            return await _repository.UpdateAsync(notification);
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            return await _repository.DeleteAsync(notificationId);
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _repository.GetByIdAsync(notificationId);
            if (notification == null) return false;

            notification.IsSent = true;
            notification.UpdatedAt = DateTime.UtcNow;
            return await _repository.UpdateAsync(notification);
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var notifications = await GetUserNotificationsAsync(userId);
            foreach (var notification in notifications)
            {
                notification.IsSent = true;
                notification.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(notification);
            }
            return true;
        }
    }
} 