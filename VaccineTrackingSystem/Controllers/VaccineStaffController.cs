using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using VaccineTrakingSystem.BLL.DTOs;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VaccineStaffController(IVaccineScheduleService VaccineSchedules,IVaccineRecordService vaccineRecord, IHttpContextAccessor httpContextAccessor)
        {
            _services = VaccineSchedules;
            _recordService = vaccineRecord;
            _httpContextAccessor = httpContextAccessor;
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
        public async Task<IActionResult> Create([FromBody] ScheduleDTO input)
        {
            var model = new VaccinationRecord
            {
                VaccinationDate = input.VaccinationDate,
                AdverseReaction = input.AdverseReaction,
                AppointmentId = input.AppointmentId,
                ChildId = input.ChildId,
                VaccineId = input.VaccineId
            };
            if (model == null || model.AppointmentId == 0)
                return BadRequest("Dữ liệu không hợp lệ");

            var userRole = _httpContextAccessor?.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine("user: " + userRole);
            model.CreatedAt = DateTime.UtcNow; // Gán ngày giờ hiện tại\
            if (int.TryParse(userRole, out int staffId))
            {
                model.StaffId = staffId;
            }
            else
            {
                // Gán giá trị mặc định hoặc báo lỗi
                model.StaffId = 0; // hoặc throw new Exception("Invalid role ID");
            }
            Console.WriteLine("Create: " + model.StaffId);
           await _recordService.CreateVaccineRecordServiceAsync(model);

            // 🌟 GỌI LẠI LOGIC CỦA VaccinationPost
            var vaccineSchedule = await _services.GetVaccineScheduleServiceByIdAsync(input.ScheduleId);
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
