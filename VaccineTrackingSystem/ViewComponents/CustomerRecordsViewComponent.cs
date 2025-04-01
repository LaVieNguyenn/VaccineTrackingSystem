using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.CenterInfoService;
using VaccineTrakingSystem.BLL.VaccineRecordService;

namespace VaccineTrackingSystem.ViewComponents
{
    public class CustomerRecordsViewComponent : ViewComponent
    {
        private readonly IVaccineRecordService _recordService;
        public CustomerRecordsViewComponent(IVaccineRecordService recordService)
        {
            _recordService = recordService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var service = await _recordService.GetAllVaccineRecordServiceAsync();
            return View(service);
        }
    }
}
