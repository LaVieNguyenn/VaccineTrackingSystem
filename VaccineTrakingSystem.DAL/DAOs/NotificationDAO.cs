using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs;

public class NotificationDAO : GenericDAO<Notification>, INotificationDAO
{
    public NotificationDAO(VaccineTrackingSystemDbContext context) : base(context)
    {
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(int userId)
    {
        return await _context.Notifications
            .Include(n => n.User)
            .Include(n => n.Child)
            .Include(n => n.Appointment)
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.NotificationDate)
            .ToListAsync();
    }

    public async Task<List<Notification>> GetUnsentNotificationsAsync()
    {
        return await _context.Notifications
            .Include(n => n.User)
            .Include(n => n.Child)
            .Include(n => n.Appointment)
            .Where(n => !n.IsSent && n.NotificationDate <= DateTime.UtcNow)
            .OrderBy(n => n.NotificationDate)
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

    public override async Task<Notification> GetByIdAsync(int id)
    {
        return await _context.Notifications
            .Include(n => n.User)
            .Include(n => n.Child)
            .Include(n => n.Appointment)
            .FirstOrDefaultAsync(n => n.NotificationId == id);
    }
} 