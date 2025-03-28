using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.BLL.VaccineScheduleService;

namespace VaccineTrackingSystem.ViewComponents
{
    public class VaccineStaffViewComponent : ViewComponent
    {
        private readonly IVaccineScheduleService _vaccineSchedulesService;

        public VaccineStaffViewComponent(IVaccineScheduleService vaccineSchedulesService)
        {
            _vaccineSchedulesService = vaccineSchedulesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var vaccineSchedulesService = await _vaccineSchedulesService.GetAllVaccineScheduleServiceAsync();
            return View(vaccineSchedulesService);
        }
    }
}
