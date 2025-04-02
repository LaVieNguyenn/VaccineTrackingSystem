using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Models
{
    public class SearchAppointmentsViewModel
    {
        public int? AppointmentID { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Username { get; set; }
        public IEnumerable<AppointmentDTO> Appointments { get; set; } = new List<AppointmentDTO>();
    }
}
