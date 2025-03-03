using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.ServicesService;

namespace VaccineTrackingSystem.ViewComponents
{
    public class ServiceViewComponent : ViewComponent
    {
        private readonly IServicesService _servicesService;
        
        public ServiceViewComponent(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var service = await _servicesService.GetAllServicesAsync();
            return View(service);
        }
    }
}
