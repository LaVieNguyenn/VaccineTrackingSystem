using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.FeedbackService;
using System.Linq;
using System.Threading.Tasks;
using VaccineTrakingSystem.BLL.DashboardService;

namespace VaccineTrackingSystem.Controllers
{
    [Authorize(Roles = "4")] // Role 4 là Admin
    public class AdminDashboardController : Controller
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IDashboardService _dashboardService;
        public AdminDashboardController(IFeedbackService feedbackService, IDashboardService dashboardService)
        {
            _feedbackService = feedbackService;
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var feedbacks = await _feedbackService.GetAllAsync();

            // Feedback statistics
            var ratingDistribution = feedbacks.GroupBy(f => f.Rating)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .OrderBy(x => x.Rating);
            ViewBag.FeedbackLabels = ratingDistribution.Select(x => $"{x.Rating} sao");
            ViewBag.FeedbackData = ratingDistribution.Select(x => x.Count);
            ViewBag.TotalFeedbacks = feedbacks.Count();
            ViewBag.AverageRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : 0;

            // Additional dashboard stats
            ViewBag.TotalAppointments = await _dashboardService.GetTotalAppointmentsAsync();
            ViewBag.TotalCustomers = await _dashboardService.GetTotalCustomersAsync();
            ViewBag.TotalVaccinationRecords = await _dashboardService.GetTotalVaccinationRecordsAsync();

            // Appointment status distribution
            var appointmentStatusDist = await _dashboardService.GetAppointmentStatusDistributionAsync();
            ViewBag.AppointmentStatusLabels = ((IEnumerable<dynamic>)appointmentStatusDist).Select(x => $"Status {x.Status}");
            ViewBag.AppointmentStatusData = ((IEnumerable<dynamic>)appointmentStatusDist).Select(x => (int)x.Count);

            // Latest Appointments and Customers (lấy 5 dòng mới nhất)
            ViewBag.LatestAppointments = await _dashboardService.GetLatestAppointmentsAsync(5);
            ViewBag.LatestCustomers = await _dashboardService.GetLatestCustomersAsync(5);

            return View(feedbacks);
        }
    }
} 