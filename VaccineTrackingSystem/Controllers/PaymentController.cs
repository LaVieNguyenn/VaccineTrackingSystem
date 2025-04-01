using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using VaccineTrakingSystem.BLL.DTOs;
using VaccineTrakingSystem.BLL.PaymentService;
using VaccineTrakingSystem.DAL.Helper;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: /Payment/ChooseMethod?appointmentId=123&amount=50.0
        [HttpGet]
        public IActionResult ChooseMethod(int appointmentId, decimal amount, int userId)
        {
            ViewBag.AppointmentId = appointmentId;
            ViewBag.Amount = amount;
            ViewBag.UserId = userId;
            return View();
        }

        // POST: /Payment/ChooseMethod
        [HttpPost]
        public IActionResult ChooseMethod(int appointmentId, decimal amount, int userId, string paymentMethod)
        {
            // Redirect đến trang Pay với các tham số được truyền
            return RedirectToAction("Pay", new { appointmentId = appointmentId, amount = amount, userId = userId, paymentMethod = paymentMethod });
        }

        [HttpGet]
        public IActionResult Pay(int appointmentId, decimal amount, int userId, string paymentMethod)
        {
            var model = new PaymentDTO
            {
                AppointmentId = appointmentId,
                CustomerId = userId,
                Amount = amount,
                PaymentMethod = paymentMethod.Equals("Momo") ? (byte)ConstantEnums.PaymentMethod.Momo : (byte)ConstantEnums.PaymentMethod.Cash,
                PaymentStatus = (byte)ConstantEnums.PaymentStatus.Pending
            };
            return View(model);
        }

        // POST: /Payment/ProcessPayment
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentDTO payment)
        {
            if (!ModelState.IsValid)
                return View("Pay", payment);

            string paymentId = await _paymentService.ProcessMomoPaymentAsync(new Payment
            {
                AppointmentId = payment.AppointmentId,
                Amount = payment.Amount,
                CustomerId = payment.CustomerId,
                PaymentMethod = payment.PaymentMethod,
                PaymentStatus = payment.PaymentStatus
            });
            if(int.TryParse(paymentId, out var id))
            {
                TempData["Message"] = "Payment processed successfully.";
                return Redirect($"/Payment/PaymentSuccess?id={id}");
            }else 
            return Redirect(paymentId);
        }

        // GET: /Payment/PaymentSuccess?id=123
        [HttpGet]
        public IActionResult PaymentSuccess(int id)
        {
            ViewBag.PaymentId = id;
            return View();
        }

        [HttpGet]
        public IActionResult PaymentFailed(int id)
        {
            ViewBag.PaymentId = id;
            return View();
        }
    }
}
