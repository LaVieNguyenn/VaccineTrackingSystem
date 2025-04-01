using System;
using System.Collections.Generic;
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

        public IActionResult Create()
        {
            var feedback = new FeedbackDTO
            {
                CustomerId = GetCurrentUserId(),
                FeedbackDate = DateTime.UtcNow
            };
            return View(feedback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackDTO feedback)
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