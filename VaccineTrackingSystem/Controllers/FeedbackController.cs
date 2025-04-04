using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.DTOs;
using VaccineTrakingSystem.BLL.FeedbackService;

namespace VaccineTrackingSystem.Controllers
{
    [Authorize(Roles = "2")]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new InvalidOperationException("User ID not found in claims");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeedbackDTO feedback)
        {
            feedback.CustomerId = GetCurrentUserId();
            feedback.FeedbackDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    // Gọi service để lưu feedback
                    await _feedbackService.CreateAsync(feedback);
                    return Json(new { success = true, message = "Feedback submitted successfully!" });
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có
                    return Json(new { success = false, message = $"Error: {ex.Message}" });
                }
            }
            else
            {
                // Nếu model không hợp lệ, trả về lỗi
                return Json(new { success = false, message = "Please provide all required fields!" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetFeedbacksByCustomer(int? appointmentId = null)
        {
            var customerId = GetCurrentUserId(); // Lấy customerId từ Claims

            // Lấy danh sách feedback theo customerId, có thể lọc theo appointmentId nếu có
            var feedbacks = await _feedbackService.GetFeedbacksByCustomerId(customerId, appointmentId);

            return Json(feedbacks); // Trả về dữ liệu dạng JSON
        }


        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var feedbacks = await _feedbackService.GetFeedbacksByCustomerId(userId);
            return View(feedbacks);
        }

        public async Task<IActionResult> Details(int id)
        {
            var feedback = await _feedbackService.GetByIdAsync(id);
            if (feedback == null || feedback.CustomerId != GetCurrentUserId())
                return NotFound();

            return View(feedback);
        }

        public IActionResult Create(int appointmentId)
        {
            var feedback = new FeedbackDTO
            {
                AppointmentId = appointmentId,
                CustomerId = GetCurrentUserId(),
                FeedbackDate = DateTime.UtcNow
            };
            return View(feedback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateF(FeedbackDTO feedback)
        {
            feedback.CustomerId = GetCurrentUserId();
            feedback.FeedbackDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                await _feedbackService.CreateAsync(feedback);
                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var feedback = await _feedbackService.GetByIdAsync(id);
            if (feedback == null || feedback.CustomerId != GetCurrentUserId())
                return NotFound();

            return View(feedback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FeedbackDTO feedback)
        {
            if (id != feedback.FeedbackId)
                return NotFound();

            var existingFeedback = await _feedbackService.GetByIdAsync(id);
            if (existingFeedback == null || existingFeedback.CustomerId != GetCurrentUserId())
                return NotFound();

            feedback.CustomerId = GetCurrentUserId();

            if (ModelState.IsValid)
            {
                var result = await _feedbackService.UpdateAsync(feedback);
                if (!result)
                    return NotFound();

                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var feedback = await _feedbackService.GetByIdAsync(id);
            if (feedback == null || feedback.CustomerId != GetCurrentUserId())
                return NotFound();

            return View(feedback);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedback = await _feedbackService.GetByIdAsync(id);
            if (feedback == null || feedback.CustomerId != GetCurrentUserId())
                return NotFound();

            var result = await _feedbackService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
