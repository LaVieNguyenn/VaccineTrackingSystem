using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.ServicesService;

public interface INotificationService
{
    Task<Notification> CreateNotificationAsync(int userId, int? childId, int? appointmentId, string message, DateTime notificationDate);
    Task<List<Notification>> GetUserNotificationsAsync(int userId);
    Task<bool> MarkNotificationAsSentAsync(int notificationId);
    Task<bool> DeleteNotificationAsync(int notificationId);
    Task<List<Notification>> GetUnsentNotificationsAsync();
} 