using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.Repositories.PaymentRepository
{
    public interface IPaymentRepository
    {
        Task<int> CreatePaymentAsync(Payment payment);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, int status, DateTime paymentDate);
        Task<Payment?> GetPaymentByAppointmentAsync(int appointmentId);
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<AppointmentDTO>> GetFilteredAppointmentsAsync(int? appointmentId, string? paymentStatus, string? phoneNumber, DateTime? appointmentDate, string? username);


    }
}
