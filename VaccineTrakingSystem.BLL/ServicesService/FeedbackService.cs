using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.ServicesService
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IGenericRepository<Feedback> _repository;

        public FeedbackService(IGenericRepository<Feedback> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Feedback>> GetUserFeedbacksAsync(int userId)
        {
            var feedbacks = await _repository.GetAllAsync();
            return feedbacks.Where(f => f.CustomerId == userId);
        }

        public async Task<Feedback?> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _repository.GetByIdAsync(feedbackId);
        }

        public async Task<int> CreateFeedbackAsync(Feedback feedback)
        {
            feedback.CreatedAt = DateTime.UtcNow;
            feedback.UpdatedAt = DateTime.UtcNow;
            return await _repository.InsertAsync(feedback);
        }

        public async Task<bool> UpdateFeedbackAsync(Feedback feedback)
        {
            feedback.UpdatedAt = DateTime.UtcNow;
            return await _repository.UpdateAsync(feedback);
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        {
            return await _repository.DeleteAsync(feedbackId);
        }

        public async Task<double> GetAverageRatingAsync()
        {
            var feedbacks = await _repository.GetAllAsync();
            if (!feedbacks.Any()) return 0;
            return feedbacks.Average(f => f.Rating);
        }
    }
} 