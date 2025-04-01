using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineTrakingSystem.BLL.DTOs;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.FeedbackService
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackDTO>> GetAllAsync();
        Task<FeedbackDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(FeedbackDTO feedback);
        Task<bool> UpdateAsync(FeedbackDTO feedback);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<FeedbackDTO>> GetFeedbacksByCustomerId(int customerId);
        Task<IEnumerable<FeedbackDTO>> GetFeedbacksByAppointmentId(int appointmentId);
    }
} 