using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.CenterInfoService;
using VaccineTrakingSystem.BLL.ServicesService;

namespace VaccineTrackingSystem.ViewComponents
{
    public class CenterInfoViewComponent : ViewComponent
    {
        private readonly ICenterInfoService _servicesService;

        public CenterInfoViewComponent(ICenterInfoService servicesService)
        {
            _servicesService = servicesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var service = await _servicesService.GetAllCenterInfoAsync();
            return View(service);
        }
    }
}
