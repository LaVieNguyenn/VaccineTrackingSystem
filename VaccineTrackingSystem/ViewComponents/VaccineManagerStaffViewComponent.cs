using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.VaccineScheduleService;
using VaccineTrakingSystem.BLL.VaccineService;

namespace VaccineTrackingSystem.ViewComponents
{
    public class VaccineManagerStaffViewComponent : ViewComponent
    {
        private readonly IVaccineService _vaccineService;

        public VaccineManagerStaffViewComponent(IVaccineService vaccineService)
        {
            _vaccineService = vaccineService;
        }

        public async Task<IViewComponentResult> InvokeAsync() // <-- id là optional
        {
                // Nếu không có id, load danh sách (Vaccine Staff mode)
                var vaccines = await _vaccineService.GetAllVaccineServiceAsync();
                return View("VaccineManagerStaff", vaccines); // View riêng cho VaccineStaff
            
        }
    }
}
