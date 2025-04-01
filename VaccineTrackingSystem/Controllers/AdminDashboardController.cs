using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.FeedbackService;
using System.Linq;
using System.Threading.Tasks;

namespace VaccineTrackingSystem.Controllers
{
    [Authorize(Roles = "4")] // Role 4 là Admin
    public class AdminDashboardController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public AdminDashboardController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public async Task<IActionResult> Index()
        {
            var feedbacks = await _feedbackService.GetAllAsync();

            // Chuẩn bị dữ liệu cho biểu đồ rating
            var ratingDistribution = feedbacks.GroupBy(f => f.Rating)
                                            .Select(g => new { Rating = g.Key, Count = g.Count() })
                                            .OrderBy(x => x.Rating);

            ViewBag.Labels = ratingDistribution.Select(x => $"{x.Rating} sao");
            ViewBag.Data = ratingDistribution.Select(x => x.Count);

            // Thống kê tổng quan
            ViewBag.TotalFeedbacks = feedbacks.Count();
            ViewBag.AverageRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : 0;

            return View(feedbacks);
        }
    }
} 