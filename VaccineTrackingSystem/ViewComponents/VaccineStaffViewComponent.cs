using Microsoft.AspNetCore.Mvc;
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

        public async Task<IViewComponentResult> InvokeAsync(int? id) // <-- id là optional
        {
            if (id.HasValue)
            {
                // Nếu có id, load 1 lịch tiêm cụ thể (Vaccination mode)
                var vaccineSchedule = await _vaccineSchedulesService.GetVaccineScheduleServiceByIdAsync(id.Value);
                return View("Vaccination", vaccineSchedule); // View riêng cho Vaccination
            }
            else
            {
                // Nếu không có id, load danh sách (Vaccine Staff mode)
                var vaccineSchedules = await _vaccineSchedulesService.GetAllVaccineScheduleServiceAsync();
                return View("VaccineStaff", vaccineSchedules); // View riêng cho VaccineStaff
            }
        }
    }
}
