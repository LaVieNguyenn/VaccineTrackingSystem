using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.ServicesService
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task<Notification?> GetNotificationByIdAsync(int notificationId);
        Task<int> CreateNotificationAsync(Notification notification);
        Task<bool> UpdateNotificationAsync(Notification notification);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAllAsReadAsync(int userId);
    }
} 