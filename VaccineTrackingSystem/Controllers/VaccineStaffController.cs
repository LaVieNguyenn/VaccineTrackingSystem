using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.BLL.VaccineRecordService;
using VaccineTrakingSystem.BLL.VaccineScheduleService;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{

    [Route("VaccineStaff")]
    public class VaccineStaffController :Controller
    {
        private readonly IVaccineScheduleService _services;
        private readonly IVaccineRecordService _recordService;
        public VaccineStaffController(IVaccineScheduleService VaccineSchedules,IVaccineRecordService vaccineRecord)
        {
            _services = VaccineSchedules;
            _recordService = vaccineRecord;
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
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] VaccinationRecord model)
        {
            if (model == null || model.AppointmentId == 0)
                return BadRequest("Dữ liệu không hợp lệ");

            model.CreatedAt = DateTime.UtcNow; // Gán ngày giờ hiện tại

            await _recordService.CreateVaccineRecordServiceAsync(model);

            // 🌟 GỌI LẠI LOGIC CỦA VaccinationPost
            var vaccineSchedule = await _services.GetVaccineScheduleServiceByIdAsync(model.AppointmentId);
            if (vaccineSchedule == null)
                return NotFound("Không tìm thấy lịch tiêm chủng này.");

            return View("Vaccination", vaccineSchedule);
        }

        public async Task<IActionResult> Index()
        {
            var VaccineSchedules = await _services.GetAllVaccineScheduleServiceAsync();
            return View(VaccineSchedules);
        }
    }
}
