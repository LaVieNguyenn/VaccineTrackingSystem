using System;
using System.Collections.Generic;

namespace VaccineTrakingSystem.DAL.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public int? ChildId { get; set; }

    public int? AppointmentId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime NotificationDate { get; set; }

    public bool IsSent { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Child? Child { get; set; }

    public virtual User User { get; set; } = null!;
}
