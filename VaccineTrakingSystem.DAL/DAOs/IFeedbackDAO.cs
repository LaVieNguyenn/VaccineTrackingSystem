using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs;

public interface IFeedbackDAO : IGenericDAO<Feedback>
{
    Task<List<Feedback>> GetCustomerFeedbacksAsync(int customerId);
    Task<List<Feedback>> GetAppointmentFeedbacksAsync(int appointmentId);
    Task<double> GetAverageRatingAsync();
} 