using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs;

public interface INotificationDAO : IGenericDAO<Notification>
{
    Task<List<Notification>> GetUserNotificationsAsync(int userId);
    Task<List<Notification>> GetUnsentNotificationsAsync();
    Task<bool> MarkNotificationAsSentAsync(int notificationId);
} 