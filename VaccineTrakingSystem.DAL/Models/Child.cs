using System;
using System.Collections.Generic;

namespace VaccineTrakingSystem.DAL.Models;

public partial class Child
{
    public int ChildId { get; set; }

    public int ParentId { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public byte Gender { get; set; }

    public string? AdditionalInfo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual User Parent { get; set; } = null!;

    public virtual ICollection<VaccinationRecord> VaccinationRecords { get; set; } = new List<VaccinationRecord>();

    public virtual ICollection<VaccineSchedule> VaccineSchedules { get; set; } = new List<VaccineSchedule>();
}
