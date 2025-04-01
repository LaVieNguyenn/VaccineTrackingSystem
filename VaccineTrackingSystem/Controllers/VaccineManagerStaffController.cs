using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.VaccineService;
using VaccineTrackingSystem.Models;
using System.Threading.Tasks;
using X.PagedList.Extensions;
using VaccineTrakingSystem.DAL.Models;
using Newtonsoft.Json;

namespace VaccineTrackingSystem.Controllers
{
    public class VaccineManagerStaffController : Controller
    {
        private readonly IVaccineService _services;

        public VaccineManagerStaffController(IVaccineService Vaccine)
        {
            _services = Vaccine;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; // Số lượng item mỗi trang
            int pageNumber = page ?? 1;

            var vaccines = await _services.GetAllVaccineServiceAsync();
            var pagedVaccines = vaccines.ToPagedList(pageNumber, pageSize);

            return View(pagedVaccines);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVaccine([FromBody] Vaccine model)
        {
            try
            {

                model.CreatedAt = DateTime.UtcNow;
                model.UpdatedAt = DateTime.UtcNow;
                Console.WriteLine("Received Data: " + JsonConvert.SerializeObject(model));

                if (model == null || string.IsNullOrEmpty(model.VaccineName))
                {
                    Console.WriteLine("Error: Invalid input detected.");
                    return BadRequest(new { message = "Invalid input" });
                }

                await _services.CreateVaccineServiceAsync(model);
                return Ok(new { message = "Vaccine created successfully!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 ERROR: " + ex.ToString()); // Log lỗi chi tiết
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }


        // 🚀 POST: Cập nhật vaccine
        [HttpPost]
        public async Task<IActionResult> EditAjax([FromBody] Vaccine model)
        {
            if (model == null) return BadRequest("Invalid data!");


            model.UpdatedAt = DateTime.UtcNow; // Cập nhật thời gian chỉnh sửa
            
            Console.WriteLine($"🔍 Debug: {JsonConvert.SerializeObject(model)}");
            bool isUpdated = await _services.UpdateVaccineServiceAsync(model);

            if (!isUpdated)
                return BadRequest("Update failed!");

            return Ok(new { message = "Updated successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAjax([FromBody] Vaccine request)
        {
            if (request == null || request.VaccineId <= 0)
            {
                Console.WriteLine("❌ Invalid request! Received: " + (request == null ? "null" : request.VaccineId.ToString()));
                return BadRequest("Invalid request!");
            }

            Console.WriteLine($"✅ Received vaccineId: {request.VaccineId}");

            bool isDeleted = await _services.DeleteVaccineServiceAsync(request.VaccineId);
            if (!isDeleted)
                return BadRequest("Delete failed!");

            return Ok(new { message = "Deleted successfully" });
        }


    }
}
