using System;
using System.Collections.Generic;

namespace VaccineTrakingSystem.DAL.Models;

public partial class Vaccine
{
    public int VaccineId { get; set; }

    public string VaccineName { get; set; } = null!;

    public string? Description { get; set; }

    public int NumberOfDoses { get; set; }

    public string? Manufacturer { get; set; }

    public string? VaccineType { get; set; }

    public int? ExpirationPeriod { get; set; }

    public DateOnly? ProductionDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<VaccinationRecord> VaccinationRecords { get; set; } = new List<VaccinationRecord>();

    public virtual ICollection<VaccineSchedule> VaccineSchedules { get; set; } = new List<VaccineSchedule>();
}
