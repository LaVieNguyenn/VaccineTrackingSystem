using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.ServicesService;

namespace VaccineTrackingSystem.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServicesService _services;
        public ServiceController(IServicesService service)
        {
            _services = service;    
        }

        public IActionResult Services()
        {
            var Services = _services.GetAllServicesAsync();
            return View(Services);
        }
    }
}
