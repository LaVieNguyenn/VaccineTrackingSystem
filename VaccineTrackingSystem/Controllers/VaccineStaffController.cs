using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.BLL.VaccineScheduleService;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{
    public class VaccineStaffController :Controller
    {
        private readonly IVaccineScheduleService _services;
        public VaccineStaffController(IVaccineScheduleService VaccineSchedules)
        {
            _services = VaccineSchedules;
        }

        public IActionResult Services()
        {
            var VaccineSchedules = _services.GetAllVaccineScheduleServiceAsync();
            return View(VaccineSchedules);
        }
        public async Task<IActionResult> Index()
        {
            var VaccineSchedules = await _services.GetAllVaccineScheduleServiceAsync();
            return View(VaccineSchedules);
        }
    }
}
