using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using VaccineTrakingSystem.BLL.Services;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.BLL.VaccineScheduleService;
using VaccineTrakingSystem.BLL.VaccineService;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{
    [Route("Child")]
    [Authorize(Roles = "2")]
    public class ChildController : Controller
    {
        private readonly IChildService _childService;
        private readonly IVaccineService _vaccineService;
        private readonly IVaccineScheduleService _vaccineScheduleService;
        private readonly IAppointmentService _appointmentService;


        public ChildController(IChildService childService, IVaccineService vaccineService, IVaccineScheduleService vaccineScheduleService, IAppointmentService appointmentService)
        {
            _childService = childService;
            _vaccineService = vaccineService;
            _vaccineScheduleService = vaccineScheduleService;
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy UserId của người dùng đăng nhập
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Chỉ lấy danh sách trẻ em của parent hiện tại
                var children = await _childService.GetChildrenByParentIdAsync(userId);
                if (children == null)
                {
                    Console.WriteLine("GetChildrenByParentIdAsync returned null");
                    return StatusCode(500, "Không thể tải danh sách trẻ em");
                }
                Console.WriteLine($"Loaded {children.Count()} children for parent ID {userId}");
                return View(children);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Index: {ex.Message}");
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Child model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.FullName))
                {
                    return BadRequest(new { Message = "Dữ liệu không hợp lệ" });
                }

                // Lấy UserId của người dùng đăng nhập và gán vào ParentId
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                model.ParentId = userId;

                model.CreatedAt = DateTime.UtcNow;
                model.UpdatedAt = DateTime.UtcNow;

                var newChildId = await _childService.AddChildAsync(model);

                if (newChildId > 0)
                {
                    return Ok(new { Message = "Thêm trẻ em thành công", ChildId = newChildId });
                }

                return StatusCode(500, new { Message = "Lỗi hệ thống khi thêm trẻ em" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create: {ex.Message}");
                return StatusCode(500, new { Message = "Lỗi hệ thống", Error = ex.Message });
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Child model)
        {
            try
            {
                if (model == null || model.ChildId <= 0)
                {
                    return BadRequest(new { Message = "Dữ liệu không hợp lệ" });
                }

                // Lấy thông tin child hiện tại để giữ nguyên ParentId
                var existingChild = await _childService.GetChildByIdAsync(model.ChildId);
                if (existingChild == null)
                {
                    return NotFound(new { Message = "Không tìm thấy thông tin trẻ em để cập nhật" });
                }

                // Kiểm tra quyền: chỉ parent của child này mới được cập nhật
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (existingChild.ParentId != userId)
                {
                    return Forbid();
                }

                // Giữ nguyên ParentId
                model.ParentId = existingChild.ParentId;

                // Cập nhật thời gian chỉnh sửa
                model.UpdatedAt = DateTime.UtcNow;

                var success = await _childService.UpdateChildAsync(model);

                if (success)
                {
                    return Ok(new { Message = "Cập nhật thông tin trẻ em thành công" });
                }

                return NotFound(new { Message = "Không tìm thấy thông tin trẻ em để cập nhật" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                return StatusCode(500, new { Message = "Lỗi hệ thống", Error = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID không hợp lệ");

            var success = await _childService.DeleteChildAsync(id);

            if (success)
                return Ok("Xóa thông tin trẻ em thành công");

            return NotFound("Không tìm thấy thông tin trẻ em để xóa");
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var child = await _childService.GetChildByIdAsync(id);
            if (child == null)
                return NotFound("Không tìm thấy thông tin trẻ em");

            return View(child);
        }

        [HttpPost("GenerateSchedule/{childId}")]
        public async Task<IActionResult> GenerateSchedule(int childId)
        {
            try
            {
                // Lấy thông tin trẻ
                var child = await _childService.GetChildByIdAsync(childId);
                if (child == null)
                {
                    Console.WriteLine($"Child with ID {childId} not found.");
                    return NotFound(new { Message = "Không tìm thấy trẻ" });
                }

                // Tính tuổi của trẻ bằng tháng
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                int ageInMonths = CalculateAgeInMonths(child.DateOfBirth, today);
                Console.WriteLine($"Child age in months: {ageInMonths}");

                // Lấy danh sách vắc-xin
                var vaccines = await _vaccineService.GetAllVaccineServiceAsync();
                if (!vaccines.Any())
                {
                    Console.WriteLine("No vaccines available in the system.");
                    return BadRequest(new { Message = "Không có vaccine nào trong hệ thống" });
                }
                Console.WriteLine($"Found {vaccines.Count()} vaccines in the system.");

                var schedules = new List<VaccineSchedule>();
                var vaccineAgeRules = GetVaccineAgeRules();
                bool hasEligibleVaccines = false;

                foreach (var vaccine in vaccines)
                {
                    Console.WriteLine($"Checking vaccine: {vaccine.VaccineName} (ID: {vaccine.VaccineId})");
                    if (vaccineAgeRules.TryGetValue(vaccine.VaccineName.ToLower(), out var ageRule))
                    {
                        Console.WriteLine($"Vaccine {vaccine.VaccineName} matched rule: {ageRule.MinAgeMonths}-{ageRule.MaxAgeMonths}");
                        // Kiểm tra xem trẻ có nằm trong độ tuổi cần tiêm không
                        if (ageInMonths >= ageRule.MinAgeMonths && ageInMonths <= ageRule.MaxAgeMonths)
                        {
                            hasEligibleVaccines = true;
                            Console.WriteLine($"Age {ageInMonths} is within range for {vaccine.VaccineName}");
                            // Kiểm tra lịch tiêm đã tồn tại
                            var existingSchedules = await _vaccineScheduleService.GetAllVaccineScheduleServiceAsync();
                            int existingDoses = existingSchedules.Count(s => s.ChildId == childId && s.VaccineId == vaccine.VaccineId);

                            // Tạo lịch cho các liều còn lại
                            for (int dose = existingDoses + 1; dose <= vaccine.NumberOfDoses; dose++)
                            {
                                // Tính ngày tiêm: mỗi liều cách nhau 2 tháng (có thể điều chỉnh)
                                var scheduledDate = today.AddMonths(2 * (dose - 1));
                                var schedule = new VaccineSchedule
                                {
                                    ChildId = childId,
                                    VaccineId = vaccine.VaccineId,
                                    ScheduledDate = scheduledDate,
                                    Status = 0, // Chưa tiêm
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow
                                };
                                schedules.Add(schedule);
                                var scheduleId = await _vaccineScheduleService.CreateVaccineScheduleServiceAsyncc(schedule);
                                Console.WriteLine($"Created schedule ID {scheduleId} for vaccine {vaccine.VaccineName} (Dose {dose}) on {schedule.ScheduledDate}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Age {ageInMonths} is outside range for {vaccine.VaccineName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No age rule found for vaccine: {vaccine.VaccineName}");
                    }
                }

                // Nếu không tạo được lịch, trả về lý do
                if (!schedules.Any())
                {
                    if (!hasEligibleVaccines)
                    {
                        return Ok(new { Message = $"Không tạo được lịch tiêm vì tuổi của trẻ ({ageInMonths} tháng) không nằm trong khoảng quy định.", Schedules = schedules });
                    }
                    else
                    {
                        return Ok(new { Message = "Không tạo được lịch tiêm vì tất cả các vắc-xin phù hợp đã có lịch.", Schedules = schedules });
                    }
                }

                return Ok(new { Message = "Lịch tiêm chủng đã được tạo", Schedules = schedules });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GenerateSchedule: {ex.Message}");
                return StatusCode(500, new { Message = "Lỗi hệ thống", Error = ex.Message });
            }
        }

        [HttpGet("Schedules/{childId}")]
        public async Task<IActionResult> GetSchedules(int childId)
        {
            try
            {
                var childSchedules = await _vaccineScheduleService.GetVaccineSchedulesByChildIdAsync(childId);
                return Ok(new { Message = "Danh sách lịch tiêm chủng", schedules = childSchedules });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetSchedules: {ex.Message}");
                return StatusCode(500, new { Message = "Lỗi hệ thống", Error = ex.Message });
            }
        }

        // Hàm tính tuổi bằng tháng
        private int CalculateAgeInMonths(DateOnly birthDate, DateOnly currentDate)
        {
            int years = currentDate.Year - birthDate.Year;
            int months = currentDate.Month - birthDate.Month;

            if (currentDate.Day < birthDate.Day)
            {
                months--;
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            return years * 12 + months;
        }

        // Định nghĩa quy tắc độ tuổi cho vắc-xin
        private Dictionary<string, (int MinAgeMonths, int MaxAgeMonths)> GetVaccineAgeRules()
        {
            return new Dictionary<string, (int MinAgeMonths, int MaxAgeMonths)>
        {
            { "mmr", (12, 72) },    // MMR: 12 tháng đến 6 tuổi (liều 1: 12-15 tháng, liều 2: 4-6 tuổi)
            { "dtap", (2, 72) }     // DTaP: 2 tháng đến 6 tuổi (liều 1-3: 2-6 tháng, liều 4: 15-18 tháng, liều 5: 4-6 tuổi)
        };
        }
        [HttpGet("Appointments/{childId}")]
        public async Task<IActionResult> GetAppointments(int childId)
        {
            try
            {
                // Kiểm tra xem trẻ có tồn tại không
                var child = await _childService.GetChildByIdAsync(childId);
                if (child == null)
                {
                    return NotFound(new { Message = "Không tìm thấy trẻ" });
                }

                // Lấy danh sách các cuộc hẹn của trẻ
                var appointments = await _appointmentService.GetAppointmentByIdAsync(childId);
                return Ok(new { Message = "Danh sách lịch hẹn", appointments = appointments }); // Sử dụng key "appointments" (chữ thường)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAppointments: {ex.Message}");
                return StatusCode(500, new { Message = "Lỗi hệ thống", Error = ex.Message });
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var children = await _childService.GetChildrenByParentIdAsync(userId);
            if (children == null)
            {
                Console.WriteLine("GetChildrenByParentIdAsync returned null");
                return StatusCode(500, new { Message = "Không thể tải danh sách trẻ em" });
            }
            Console.WriteLine($"Loaded {children.Count()} children for parent ID {userId}");
            return Ok(children); // Trả về danh sách trẻ em dưới dạng JSON
        }
    }
}
