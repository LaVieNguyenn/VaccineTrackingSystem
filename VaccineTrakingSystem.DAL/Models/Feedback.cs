using System;
using System.Collections.Generic;

namespace VaccineTrakingSystem.DAL.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? AppointmentId { get; set; }

    public int CustomerId { get; set; }

    public int Rating { get; set; }

    public string? Comments { get; set; }

    public DateTime FeedbackDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual User Customer { get; set; } = null!;
}
