using System;
using System.Collections.Generic;

namespace VaccineTrakingSystem.DAL.Models;

public partial class VaccineSchedule
{
    public int ScheduleId { get; set; }

    public int ChildId { get; set; }

    public int VaccineId { get; set; }

    public DateOnly ScheduledDate { get; set; }

    public byte Status { get; set; }

    public int? AppointmentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Child Child { get; set; } = null!;

    public virtual Vaccine Vaccine { get; set; } = null!;
}
