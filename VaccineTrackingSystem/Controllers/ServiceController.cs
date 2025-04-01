using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServicesService _services;
        public ServiceController(IServicesService service)
        {
            _services = service;    
        }

        public async Task<IActionResult> Services()
        {
            var services = await _services.GetAllServicesAsync();
            return View(services);
        }
        [HttpGet]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _services.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound(new { success = false, message = "Service not found" });
            }
            return Json(service);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateService([FromBody] Service service)
        {
            if (service == null)
            {
                return BadRequest(new { success = false, message = "Invalid service data" });
            }

            service.UpdatedAt = DateTime.Now;
            var isUpdated = await _services.UpdateServiceAsync(service);

            if (isUpdated)
            {
                return Json(new { success = true, message = "Service updated successfully" });
            }
            return Json(new { success = false, message = "Failed to update service" });
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteService(int id)
        {
            Console.WriteLine($"Deleting service with ID: {id}");

            var service = await _services.GetServiceByIdAsync(id);
            if (service == null)
            {
                Console.WriteLine("service not found");
                return NotFound(new { success = false, message = "service not found" });
            }

            var isDeleted = await _services.DeleteServiceAsync(id);
            if (isDeleted)
            {
                Console.WriteLine("service deleted successfully");
                return Json(new { success = true, message = "service deleted successfully" });
            }

            Console.WriteLine("Failed to delete service");
            return Json(new { success = false, message = "Failed to delete service" });
        }
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] Service service)
        {
            try
            {
                int newServiceId = await _services.CreateServiceAsync(service);
                if (newServiceId > 0)
                {
                    return Json(new { success = true, message = "User created successfully", serviceId = newServiceId });
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
