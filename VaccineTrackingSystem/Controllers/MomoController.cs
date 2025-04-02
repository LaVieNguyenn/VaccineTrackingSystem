using Client.Logics.Commons.MomoLogics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.BLL.PaymentService;
using VaccineTrakingSystem.BLL.Services;
using VaccineTrakingSystem.DAL.Helper;

namespace Client.Controllers.V1.ThirdParty
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MomoController : ControllerBase
    {
        private readonly IMomoService _momoService;
        private readonly IPaymentService  _paymentService;
        private readonly IAppointmentService _appointmentService;

        public MomoController(IMomoService momoService, IPaymentService  paymentService, IAppointmentService appointmentService)
        {
            _momoService = momoService;
            _paymentService = paymentService;
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            string trueUrl = "/Payment/PaymentSuccess?id=";
            string falseUrl = "/Payment/PaymentFailed?id=";

            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            var payment = _paymentService.GetPaymentByIdAsync(int.Parse(response.OrderId));
            if (payment.Result == null) return Redirect(falseUrl);
            if(response.ErrorCode ==  "0")
            {
                payment.Result.PaymentStatus = (byte)ConstantEnums.PaymentStatus.Paid;
                _paymentService.UpdatePaymentStatusAsync(payment.Result.PaymentId, (byte)ConstantEnums.PaymentStatus.Paid, DateTime.Now);
                var appoitment = _appointmentService.GetAppointmentByIdAsync(payment.Result.AppointmentId);
                appoitment.Result.PaymentStatus = (byte)ConstantEnums.PaymentStatus.Paid;
                _appointmentService.UpdateAppointmentAsync(appoitment.Result);
                return Redirect(trueUrl + $"{payment.Result.PaymentId}");
            }
            return Redirect(falseUrl + $"{payment.Result.PaymentId}");
        }
    }
}