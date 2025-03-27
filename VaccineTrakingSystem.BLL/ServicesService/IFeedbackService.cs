using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.ServicesService
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<IEnumerable<Feedback>> GetUserFeedbacksAsync(int userId);
        Task<Feedback?> GetFeedbackByIdAsync(int feedbackId);
        Task<int> CreateFeedbackAsync(Feedback feedback);
        Task<bool> UpdateFeedbackAsync(Feedback feedback);
        Task<bool> DeleteFeedbackAsync(int feedbackId);
        Task<double> GetAverageRatingAsync();
    }
} 