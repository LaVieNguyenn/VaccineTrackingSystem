using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.PaymentService
{
    public interface IPaymentService
    {
        Task<string> ProcessMomoPaymentAsync(Payment payment);
        Task<Payment?> GetPaymentByAppointmentAsync(int appointmentId);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, int status, DateTime paymentDate);
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<AppointmentDTO>> GetUnpaidAppointmentsAsync(int? appointmentId, string? phoneNumber, string? username);
    }
}
