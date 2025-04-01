    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VaccineTrakingSystem.BLL.Services;
    using VaccineTrakingSystem.BLL.ServicesService;
    using VaccineTrakingSystem.DAL.Models;

    namespace VaccineTrackingSystem.Controllers
    {
        [Route("Appointment")]
        [Authorize(Roles = "2")]
        public class AppointmentController : Controller
        {
            private readonly IAppointmentService _appointmentService;
            private readonly IServicesService _services;
            private readonly IChildService _childService; // Thêm IChildService

            public AppointmentController(IAppointmentService appointmentService, IServicesService servicesService, IChildService childService)
            {
                _appointmentService = appointmentService;
                _services = servicesService;
                _childService = childService; // Khởi tạo IChildService
            }

            [HttpGet]
            public async Task<IActionResult> Index()
            {
                try
                {
                    // Lấy danh sách cuộc hẹn
                    var appointments = await _appointmentService.GetAllAppointmentsAsync();

                    // Lấy danh sách trẻ em để ánh xạ ChildId sang ChildName
                    var children = await _childService.GetAllChildrenAsync();
                    var childDictionary = children.ToDictionary(c => c.ChildId, c => c.FullName);

                    // Truyền childDictionary vào ViewData để sử dụng trong view
                    ViewData["ChildDictionary"] = childDictionary;

                    return View(appointments);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Index: {ex.Message}");
                    return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
                }
            }

            [HttpPost("Create")]
            public async Task<IActionResult> Create([FromBody] Appointment model)
            {
                try
                {
                    if (model == null || model.ChildId == 0 || model.ServiceId == 0)
                    {
                        return BadRequest(new { Message = "Dữ liệu không hợp lệ" });
                    }

                    if (model.BookingDate == default || model.BookingDate < new DateTime(1753, 1, 1))
                    {
                        model.BookingDate = DateTime.UtcNow;
                    }

                    model.AppointmentDate = model.AppointmentDate == default || model.AppointmentDate < new DateTime(1753, 1, 1)
                        ? model.BookingDate
                        : model.AppointmentDate;

                    model.CreatedAt = model.CreatedAt == default ? DateTime.UtcNow : model.CreatedAt;
                    model.UpdatedAt = model.UpdatedAt == default ? DateTime.UtcNow : model.UpdatedAt;

                    if (model.Status != 0 && model.Status != 1)
                    {
                        model.Status = 0; // Mặc định là Pending
                    }
                    model.PaymentStatus = 0; // Mặc định là Unpaid

                    Console.WriteLine($"BookingDate: {model.BookingDate}");
                    Console.WriteLine($"AppointmentDate: {model.AppointmentDate}");
                    Console.WriteLine($"CreatedAt: {model.CreatedAt}");
                    Console.WriteLine($"UpdatedAt: {model.UpdatedAt}");

                    var newAppointmentId = await _appointmentService.BookAppointmentAsync(model);

                    if (newAppointmentId > 0)
                    {
                        return Ok(new { Message = "Đặt lịch thành công", AppointmentId = newAppointmentId });
                    }

                    return StatusCode(500, new { Message = "Lỗi hệ thống khi tạo lịch hẹn" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Create: {ex.Message}");
                    return StatusCode(500, new { Message = "Lỗi hệ thống", Error = ex.Message });
                }
            }

            [HttpPut("Update")]
            public async Task<IActionResult> Update([FromBody] Appointment model)
            {
                try
                {
                    if (model == null || model.AppointmentId <= 0)
                    {
                        return BadRequest(new { Message = "Dữ liệu không hợp lệ" });
                    }

                    if (model.ChildId <= 0 || model.ServiceId <= 0)
                    {
                        return BadRequest(new { Message = "ChildId hoặc ServiceId không hợp lệ" });
                    }

                    if (model.BookingDate == default || model.BookingDate < new DateTime(1753, 1, 1))
                    {
                        model.BookingDate = DateTime.UtcNow;
                    }
                    if (model.AppointmentDate == default || model.AppointmentDate < new DateTime(1753, 1, 1))
                    {
                        model.AppointmentDate = model.BookingDate;
                    }
                    if (model.CreatedAt == default || model.CreatedAt < new DateTime(1753, 1, 1))
                    {
                        model.CreatedAt = DateTime.UtcNow;
                    }
                    model.UpdatedAt = DateTime.UtcNow;

                    Console.WriteLine($"Received model: AppointmentId={model.AppointmentId}, ChildId={model.ChildId}, ServiceId={model.ServiceId}, BookingDate={model.BookingDate}, AppointmentDate={model.AppointmentDate}");

                    var success = await _appointmentService.UpdateAppointmentAsync(model);

                    if (success)
                    {
                        return Ok(new { Message = "Cập nhật lịch hẹn thành công" });
                    }

                    return NotFound(new { Message = "Không tìm thấy lịch hẹn để cập nhật" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Update: {ex.Message}");
                    return StatusCode(500, new { Message = "Lỗi hệ thống", Error = ex.Message });
                }
            }

            [HttpDelete("Cancel/{id}")]
            public async Task<IActionResult> Cancel(int id)
            {
                if (id <= 0)
                    return BadRequest("ID không hợp lệ");

                var success = await _appointmentService.CancelAppointmentAsync(id);

                if (success)
                    return Ok("Hủy lịch hẹn thành công");

                return NotFound("Không tìm thấy lịch hẹn để hủy");
            }

            [HttpGet("Detail/{id}")]
            public async Task<IActionResult> Detail(int id)
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                if (appointment == null)
                    return NotFound("Không tìm thấy lịch hẹn");

                return View(appointment);
            }

            [HttpGet("GetAll")]
            public async Task<IActionResult> GetAll()
            {
                try
                {
                    var services = await _services.GetAllServicesAsync();
                    return Ok(services);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetAll: {ex.Message}");
                    return StatusCode(500, new { Message = "Lỗi hệ thống", Error = ex.Message });
                }
            }
        }
    }