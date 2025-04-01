using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.DAOs.PaymentDAO;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.Repositories.PaymentRepository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IPaymentDAO _paymentDAO;
        public PaymentRepository(IPaymentDAO paymentDAO)
        {
            _paymentDAO = paymentDAO;
        }

        public Task<int> CreatePaymentAsync(Payment payment)
        {
            return _paymentDAO.InsertPaymentAsync(payment);
        }

        public Task<bool> UpdatePaymentStatusAsync(int paymentId, int status, DateTime paymentDate)
        {
            return _paymentDAO.UpdatePaymentStatusAsync(paymentId, status, paymentDate);
        }

        public Task<Payment?> GetPaymentByAppointmentAsync(int appointmentId)
        {
            return _paymentDAO.GetPaymentByAppointmentAsync(appointmentId);
        }

        public Task<Payment?> GetPaymentByIdAsync(int id) => _paymentDAO.GetPaymentByIdAsync(id);

    }
}
