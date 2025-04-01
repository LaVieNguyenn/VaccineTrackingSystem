using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VaccineTrakingSystem.BLL.Services;

namespace VaccineTrackingSystem.ViewComponents
{
    public class AppointmentViewComponent : ViewComponent
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentViewComponent(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }
    }
}
