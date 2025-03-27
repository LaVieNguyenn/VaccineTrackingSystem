using System;
using System.Collections.Generic;

namespace VaccineTrakingSystem.DAL.Models;

public partial class CenterInfo
{
    public int CenterId { get; set; }

    public string CenterName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string ContactInfo { get; set; } = null!;

    public string? Description { get; set; }

    public string? ServiceDetails { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
