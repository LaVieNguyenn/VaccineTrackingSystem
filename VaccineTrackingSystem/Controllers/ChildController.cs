using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VaccineTrakingSystem.BLL.ChildService;
using VaccineTrakingSystem.BLL.Services;
using VaccineTrakingSystem.BLL.VaccinationRecordService;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{
    public class ChildController : Controller
    {
        private readonly IChildService _childService;
        private readonly IAppointmentService _appointmentService;
        private readonly IVaccinationRecordService _vaccinationRecordService;

        public ChildController(
            IChildService childService,
            IAppointmentService appointmentService,
            IVaccinationRecordService vaccinationRecordService)
        {
            _childService = childService;
            _appointmentService = appointmentService;
            _vaccinationRecordService = vaccinationRecordService;
        }

        public async Task<IActionResult> Index()
        {
            var children = await _childService.GetAllChildrenAsync();
            return View(children);
        }

        public async Task<IActionResult> Details(int id)
        {
            var child = await _childService.GetChildByIdAsync(id);
            if (child == null)
            {
                return NotFound();
            }
            return View(child);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Child model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _childService.CreateChildAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var child = await _childService.GetChildByIdAsync(id);
            if (child == null)
            {
                return NotFound();
            }
            return View(child);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Child model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _childService.UpdateChildAsync(model);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to update child record.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _childService.DeleteChildAsync(id);
            return Json(new { success = result });
        }

        public async Task<IActionResult> AppointmentHistory(int id)
        {
            var appointments = await _appointmentService.GetAppointmentsByChildIdAsync(id);
            if (appointments == null)
            {
                return NotFound();
            }
            return View(appointments);
        }

        public async Task<IActionResult> VaccinationHistory(int id)
        {
            var records = await _vaccinationRecordService.GetVaccinationRecordsByChildIdAsync(id);
            if (records == null)
            {
                return NotFound();
            }
            return View(records);
        }
    }
}
