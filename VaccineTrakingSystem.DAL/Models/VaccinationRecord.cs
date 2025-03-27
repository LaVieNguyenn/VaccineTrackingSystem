using System;
using System.Collections.Generic;

namespace VaccineTrakingSystem.DAL.Models;

public partial class VaccinationRecord
{
    public int RecordId { get; set; }

    public int AppointmentId { get; set; }

    public int ChildId { get; set; }

    public int VaccineId { get; set; }

    public DateOnly VaccinationDate { get; set; }

    public string? AdverseReaction { get; set; }

    public int StaffId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Child Child { get; set; } = null!;

    public virtual User Staff { get; set; } = null!;

    public virtual Vaccine Vaccine { get; set; } = null!;
}
