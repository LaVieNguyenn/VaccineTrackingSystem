using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineTrakingSystem.BLL.DTOs
{
    public class AppointmentDTO
    {
        public int AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
