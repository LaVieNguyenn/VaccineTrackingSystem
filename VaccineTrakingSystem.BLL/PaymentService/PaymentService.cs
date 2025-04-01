using Client.Controllers.V1.ThirdParty;
using Client.Logics.Commons.MomoLogics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Helper;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories.PaymentRepository;

namespace VaccineTrakingSystem.BLL.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMomoService _momoService;
        public PaymentService(IPaymentRepository paymentRepository, IMomoService momoService)
        {
            _paymentRepository = paymentRepository;
            _momoService = momoService;
        }

        public Task<Payment?> GetPaymentByAppointmentAsync(int appointmentId) => _paymentRepository.GetPaymentByAppointmentAsync(appointmentId);

        public Task<Payment?> GetPaymentByIdAsync(int id) => _paymentRepository.GetPaymentByIdAsync(id);

        public async Task<string> ProcessMomoPaymentAsync(Payment payment)
        {
            payment.CreatedAt = DateTime.Now;
            payment.UpdatedAt = DateTime.Now;
            payment.PaymentDate = DateTime.Now;
            payment.PaymentStatus = (byte)ConstantEnums.PaymentStatus.Pending;
            int paymentId = await _paymentRepository.CreatePaymentAsync(payment);

            if (payment.PaymentMethod == (byte)ConstantEnums.PaymentMethod.Momo)
            {
                var momoRequest = new MomoExecuteResponseModel
                {
                    // Sử dụng Payment.Amount và AppointmentID để tạo OrderInfo nếu cần
                    Amount = (Math.Round(payment.Amount)*1000).ToString(),
                    OrderInfo = $"Payment for appointment {payment.AppointmentId}",
                    OrderId = paymentId.ToString() + "_" + DateTime.UtcNow.Ticks.ToString(),
                    FullName = payment.CustomerId.ToString(),
                };

                 var result = _momoService.CreatePaymentAsync(momoRequest).Result.PayUrl.ToString();
                return result;
            }
            else if (payment.PaymentMethod == (byte)ConstantEnums.PaymentMethod.Cash)
            {
                // Với tiền mặt, ta giả định thanh toán được xử lý trực tiếp
                await _paymentRepository.UpdatePaymentStatusAsync(paymentId, (byte)ConstantEnums.PaymentStatus.Paid, DateTime.Now);
            }
            return paymentId.ToString();
        }

        public Task<bool> UpdatePaymentStatusAsync(int paymentId, int status, DateTime paymentDat) => _paymentRepository.UpdatePaymentStatusAsync(paymentId, status, paymentDat);
    }
}
