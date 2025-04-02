using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using VaccineTrackingSystem.Models;
using VaccineTrakingSystem.BLL.DTOs;
using VaccineTrakingSystem.BLL.PaymentService;
using VaccineTrakingSystem.BLL.Services;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.DAL.Helper;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IServicesService _servicesService;
        private readonly IAppointmentService _appointmentService;
        private readonly IChildService _childService;
        public PaymentController(IPaymentService paymentService, IServicesService servicesService, IAppointmentService appointmentService, IChildService childService)
        {
            _paymentService = paymentService;
            _servicesService = servicesService;
            _appointmentService = appointmentService;
            _childService = childService;
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

        [HttpGet]
        public async Task<IActionResult> Index(int? appointmentId, string? phoneNumber, string? username)
        {
            var appointments = await _paymentService.GetUnpaidAppointmentsAsync(appointmentId, phoneNumber, username);
            var viewModel = new SearchAppointmentsViewModel
            {
                AppointmentID = appointmentId,
                PhoneNumber = phoneNumber,
                Username = username,
                Appointments = appointments
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> SelectForPayment(int appointmentId)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            var service = await _servicesService.GetServiceByIdAsync(appointment.ServiceId);
            var child = await _childService.GetChildByIdAsync(appointment.ChildId);
            return Redirect($"/Payment/ChooseMethod?appointmentId={appointment.AppointmentId}&amount={service.Price}&userId={child.ParentId}");
        }
    }
}
