    using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VaccineTrakingSystem.BLL.VaccineRecordService;
using VaccineTrakingSystem.BLL.VaccineService;
using VaccineTrakingSystem.DAL.Models;
using X.PagedList.Extensions;

namespace VaccineTrackingSystem.Controllers
{
    public class CustomerRecordsController : Controller
    {

        private readonly IVaccineRecordService _services;
        public CustomerRecordsController(IVaccineRecordService CSRecords)
        {
            _services = CSRecords;
        }
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; // Số lượng item mỗi trang
            int pageNumber = page ?? 1;

            var records = await _services.GetAllVaccineRecordServiceAsync();
            foreach (var r in records)
            {
                Console.WriteLine($"Vaccine: {r.Vaccine?.VaccineId}, Child: {r.Child?.FullName}, Staff: {r.Staff?.FullName}, Date: {r.VaccinationDate}");
            }

            var pagedRecords = records.ToPagedList(pageNumber, pageSize);

            return View(pagedRecords);
        }
        [HttpPost]
        public async Task<IActionResult> AddReaction([FromBody] VaccinationRecord model)
        {
            if (model == null) return BadRequest(new { success = false, message = "Invalid data!" });

            // Lấy bản ghi tiêm chủng cũ từ cơ sở dữ liệu dựa trên RecordId
            var existingRecord = await _services.GetVaccineRecordServiceByIdAsync(model.RecordId);

            if (existingRecord == null)
                return NotFound(new { success = false, message = "Vaccination record not found!" });

            // Lấy ghi chú phản ứng bất lợi cũ và thêm phản ứng mới
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vnTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            var currentDate = vnTime.ToString("yyyy-MM-dd HH:mm:ss");

            var newAdverseReaction = $"Customer Reported date: {currentDate}\nAdverseReaction: {model.AdverseReaction}";

            // Cập nhật ghi chú cũ với thông tin phản ứng mới
            existingRecord.AdverseReaction = string.IsNullOrEmpty(existingRecord.AdverseReaction)
                ? newAdverseReaction
                : existingRecord.AdverseReaction + "\n\n" + newAdverseReaction;

            // Cập nhật thời gian chỉnh sửa
            existingRecord.UpdatedAt = DateTime.UtcNow;

            // Cập nhật bản ghi trong cơ sở dữ liệu
            bool isUpdated = await _services.UpdateVaccineRecordServiceAsync(existingRecord);

            if (!isUpdated)
                return BadRequest(new { success = false, message = "Update failed!" });

            return Ok(new { success = true, message = "Adverse reaction added successfully" });
        }


        // Thêm phương thức GetVaccineRecordById vào controller để lấy bản ghi
        [HttpGet]
        public async Task<IActionResult> GetVaccineRecordById(int id)
        {
            var record = await _services.GetVaccineRecordServiceByIdAsync(id);
            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

    }
}
