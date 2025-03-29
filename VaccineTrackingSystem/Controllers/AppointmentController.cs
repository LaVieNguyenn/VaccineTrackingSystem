using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.Services;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (!ModelState.IsValid)
                return View(appointment);

            await _appointmentService.CreateAppointmentAsync(appointment);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();

            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Appointment appointment)
        {
            if (!ModelState.IsValid)
                return View(appointment);

            var updated = await _appointmentService.UpdateAppointmentAsync(appointment);
            if (!updated) return BadRequest("Update failed!");

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();

            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var deleted = await _appointmentService.DeleteAppointmentAsync(id);
            if (!deleted) return BadRequest("Delete failed!");

            return RedirectToAction("Index");
        }
    }
}
