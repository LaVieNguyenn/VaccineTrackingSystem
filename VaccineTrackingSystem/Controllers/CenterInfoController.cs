using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.CenterInfoService;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{
    [Authorize(Roles = "4")]
    public class CenterInfoController : Controller
    {
     
        private readonly ICenterInfoService _services;
        public CenterInfoController(ICenterInfoService service)
        {
            _services = service;
        }

        public async Task<IActionResult> CenterInfor()
        {
            var centerInfor = await _services.GetAllCenterInfoAsync();
            return View(centerInfor);
        }
        [HttpGet]
        public async Task<IActionResult> GetCenterInforById(int id)
        {
            var centerInfor = await _services.GetCenterInfoByIdAsync(id);
            if (centerInfor == null)
            {
                return NotFound(new { success = false, message = "CenterInfor not found" });
            }
            return Json(centerInfor);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCenterInfor([FromBody] CenterInfo centerInfor)
        {
            if (centerInfor == null)
            {
                return BadRequest(new { success = false, message = "Invalid centerInfor data" });
            }

            centerInfor.UpdatedAt = DateTime.Now;
            var isUpdated = await _services.UpdateCenterInfoAsync(centerInfor);

            if (isUpdated)
            {
                return Json(new { success = true, message = "CenterInfor updated successfully" });
            }
            return Json(new { success = false, message = "Failed to update centerInfor" });
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCenterInfo(int id)
        {
            Console.WriteLine($"Deleting centerInfor with ID: {id}");

            var centerInfor = await _services.GetCenterInfoByIdAsync(id);
            if (centerInfor == null)
            {
                Console.WriteLine("CenterInfor not found");
                return NotFound(new { success = false, message = "CenterInfor not found" });
            }

            var isDeleted = await _services.DeleteCenterInfoAsync(id);
            if (isDeleted)
            {
                Console.WriteLine("CenterInfor deleted successfully");
                return Json(new { success = true, message = "CenterInfor deleted successfully" });
            }

            Console.WriteLine("Failed to delete centerInfor");
            return Json(new { success = false, message = "Failed to delete centerInfor" });
        }
        [HttpPost]
        public async Task<IActionResult> CreateCenterInfor([FromBody] CenterInfo centerInfor)
        {
            try
            {
                int newcenterId = await _services.CreateCenterInfoAsync(centerInfor);
                if (newcenterId > 0)
                {
                    return Json(new { success = true, message = "User created successfully", CenterId = newcenterId });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

            return Json(new { success = false, message = "Failed to create user" });
        }
    }
}
