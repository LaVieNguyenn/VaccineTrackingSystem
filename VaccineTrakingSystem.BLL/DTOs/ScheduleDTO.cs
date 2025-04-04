using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineTrakingSystem.BLL.DTOs
{
    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }

        public int RecordId { get; set; }

        public int AppointmentId { get; set; }

        public int ChildId { get; set; }
        public string? ChildName { get; set; }

        public int VaccineId { get; set; }

        public DateOnly VaccinationDate { get; set; }

        public string? AdverseReaction { get; set; }
        public int? UserId { get; set; }

        public int StaffId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
