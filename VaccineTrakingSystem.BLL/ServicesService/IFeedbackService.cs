using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.ServicesService;

public interface IFeedbackService
{
    Task<Feedback> CreateFeedbackAsync(int? appointmentId, int customerId, int rating, string? comments);
    Task<Feedback> GetFeedbackAsync(int feedbackId);
    Task<List<Feedback>> GetCustomerFeedbacksAsync(int customerId);
    Task<List<Feedback>> GetAppointmentFeedbacksAsync(int appointmentId);
    Task<double> GetAverageRatingAsync();
    Task<bool> DeleteFeedbackAsync(int feedbackId);
} 