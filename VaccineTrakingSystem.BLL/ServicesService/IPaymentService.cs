using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.ServicesService;

public interface IPaymentService
{
    Task<Payment> CreatePaymentAsync(int appointmentId, int customerId, decimal amount, byte paymentMethod);
    Task<Payment> GetPaymentAsync(int paymentId);
    Task<List<Payment>> GetCustomerPaymentsAsync(int customerId);
    Task<bool> UpdatePaymentStatusAsync(int paymentId, byte paymentStatus);
    Task<List<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
} 