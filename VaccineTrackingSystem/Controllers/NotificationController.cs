using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.DTOs;
using VaccineTrakingSystem.BLL.ServicesService;

namespace VaccineTrackingSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<ActionResult<NotificationResponseDTO>> CreateNotification([FromBody] CreateNotificationDTO dto)
    {
        var notification = await _notificationService.CreateNotificationAsync(
            dto.UserId,
            dto.ChildId,
            dto.AppointmentId,
            dto.Message,
            dto.NotificationDate
        );

        return Ok(new NotificationResponseDTO
        {
            NotificationId = notification.NotificationId,
            UserId = notification.UserId,
            ChildId = notification.ChildId,
            AppointmentId = notification.AppointmentId,
            Message = notification.Message,
            NotificationDate = notification.NotificationDate,
            IsSent = notification.IsSent,
            CreatedAt = notification.CreatedAt
        });
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<NotificationResponseDTO>>> GetUserNotifications(int userId)
    {
        var notifications = await _notificationService.GetUserNotificationsAsync(userId);
        return Ok(notifications.Select(n => new NotificationResponseDTO
        {
            NotificationId = n.NotificationId,
            UserId = n.UserId,
            ChildId = n.ChildId,
            AppointmentId = n.AppointmentId,
            Message = n.Message,
            NotificationDate = n.NotificationDate,
            IsSent = n.IsSent,
            CreatedAt = n.CreatedAt
        }));
    }

    [HttpPut("{notificationId}/mark-sent")]
    public async Task<ActionResult> MarkNotificationAsSent(int notificationId)
    {
        var result = await _notificationService.MarkNotificationAsSentAsync(notificationId);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpDelete("{notificationId}")]
    public async Task<ActionResult> DeleteNotification(int notificationId)
    {
        var result = await _notificationService.DeleteNotificationAsync(notificationId);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpGet("unsent")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<NotificationResponseDTO>>> GetUnsentNotifications()
    {
        var notifications = await _notificationService.GetUnsentNotificationsAsync();
        return Ok(notifications.Select(n => new NotificationResponseDTO
        {
            NotificationId = n.NotificationId,
            UserId = n.UserId,
            ChildId = n.ChildId,
            AppointmentId = n.AppointmentId,
            Message = n.Message,
            NotificationDate = n.NotificationDate,
            IsSent = n.IsSent,
            CreatedAt = n.CreatedAt
        }));
    }
} 