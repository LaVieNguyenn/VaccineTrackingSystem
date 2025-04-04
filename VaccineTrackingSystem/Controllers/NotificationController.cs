using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using VaccineTrackingSystem.Hubs;
using VaccineTrakingSystem.BLL.NotificationService;

namespace VaccineTrackingSystem.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(INotificationService notificationService, IHubContext<NotificationHub> hubContext)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendChildUpdate(int userId, int? childId, string childName)
        {
            string message = $"Record for child '{childName}' has been updated.";
            // Lưu thông báo vào DB
            await _notificationService.CreateNotificationAsync(userId, childId, message);
            // Gửi thông báo qua SignalR đến user có userId
            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message);
            return Ok(new { success = true });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOldNotifications()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var notifications = await _notificationService.GetNotificationsByUserAsync(userId);
            return Json(notifications);
        }
    }
}
