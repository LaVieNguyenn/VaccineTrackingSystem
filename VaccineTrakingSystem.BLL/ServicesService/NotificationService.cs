using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.ServicesService;

public class NotificationService : INotificationService
{
    private readonly VaccineTrackingSystemDbContext _context;

    public NotificationService(VaccineTrackingSystemDbContext context)
    {
        _context = context;
    }

    public async Task<Notification> CreateNotificationAsync(int userId, int? childId, int? appointmentId, string message, DateTime notificationDate)
    {
        var notification = new Notification
        {
            UserId = userId,
            ChildId = childId,
            AppointmentId = appointmentId,
            Message = message,
            NotificationDate = notificationDate,
            IsSent = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(int userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.NotificationDate)
            .ToListAsync();
    }

    public async Task<bool> MarkNotificationAsSentAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification == null) return false;

        notification.IsSent = true;
        notification.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteNotificationAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification == null) return false;

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Notification>> GetUnsentNotificationsAsync()
    {
        return await _context.Notifications
            .Where(n => !n.IsSent && n.NotificationDate <= DateTime.UtcNow)
            .OrderBy(n => n.NotificationDate)
            .ToListAsync();
    }
} 