using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.PaymentDAO
{
    public interface IPaymentDAO
    {
        Task<int> InsertPaymentAsync(Payment payment);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, int status, DateTime paymentDate);
        Task<Payment?> GetPaymentByAppointmentAsync(int appointmentId);
        Task<Payment?> GetPaymentByIdAsync(int id);

    }
}
