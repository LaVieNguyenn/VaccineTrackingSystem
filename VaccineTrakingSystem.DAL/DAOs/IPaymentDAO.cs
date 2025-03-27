using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs;

public interface IPaymentDAO : IGenericDAO<Payment>
{
    Task<List<Payment>> GetCustomerPaymentsAsync(int customerId);
    Task<List<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<bool> UpdatePaymentStatusAsync(int paymentId, byte paymentStatus);
} 