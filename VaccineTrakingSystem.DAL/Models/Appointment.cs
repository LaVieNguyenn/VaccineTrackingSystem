using System;
using System.Collections.Generic;

namespace VaccineTrakingSystem.DAL.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int ChildId { get; set; }

    public int ServiceId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public DateTime BookingDate { get; set; }

    public byte Status { get; set; }

    public byte PaymentStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Child Child { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Service Service { get; set; } = null!;

    public virtual ICollection<VaccinationRecord> VaccinationRecords { get; set; } = new List<VaccinationRecord>();

    public virtual ICollection<VaccineSchedule> VaccineSchedules { get; set; } = new List<VaccineSchedule>();
}
