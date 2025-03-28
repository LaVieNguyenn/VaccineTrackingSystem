using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.BLL.VaccineScheduleService;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{

    [Route("VaccineStaff")]
    public class VaccineStaffController :Controller
    {
        private readonly IVaccineScheduleService _services;
        public VaccineStaffController(IVaccineScheduleService VaccineSchedules)
        {
            _services = VaccineSchedules;
        }

        [HttpPost("Vaccination")]
        public async Task<IActionResult> VaccinationPost([FromForm] int id)
        {
            var vaccineSchedule = await _services.GetVaccineScheduleServiceByIdAsync(id);

            if (vaccineSchedule == null)
            {
                return NotFound("Không tìm thấy lịch tiêm chủng này.");
            }

            // Debug để xem dữ liệu trước khi render View
            return View("Vaccination", vaccineSchedule);
        }



        public async Task<IActionResult> Index()
        {
            var VaccineSchedules = await _services.GetAllVaccineScheduleServiceAsync();
            return View(VaccineSchedules);
        }
    }
}
