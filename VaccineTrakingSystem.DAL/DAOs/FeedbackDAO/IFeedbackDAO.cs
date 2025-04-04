using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.FeedbackDAO
{
    public interface IFeedbackDAO : IGenericDAO<Feedback>
    {
        //Task<IEnumerable<Feedback>> GetFeedbacksByCustomerId(int customerId);
        Task<IEnumerable<Feedback>> GetFeedbacksByAppointmentId(int appointmentId);
        Task<IEnumerable<Feedback>> GetFeedbacksByCustomerId(int customerId, int? appointmentId = null);
    }
} 