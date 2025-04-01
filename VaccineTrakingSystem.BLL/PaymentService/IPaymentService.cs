using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.PaymentService
{
    public interface IPaymentService
    {
        Task<string> ProcessMomoPaymentAsync(Payment payment);
        Task<Payment?> GetPaymentByAppointmentAsync(int appointmentId);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, int status, DateTime paymentDate);
        Task<Payment?> GetPaymentByIdAsync(int id);

    }
}
