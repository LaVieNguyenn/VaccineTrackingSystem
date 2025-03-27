namespace VaccineTrakingSystem.BLL.DTOs;

public class CreateVaccinationRecordDTO
{
    public int AppointmentId { get; set; }
    public int ChildId { get; set; }
    public int VaccineId { get; set; }
    public DateOnly VaccinationDate { get; set; }
    public string? AdverseReaction { get; set; }
    public int StaffId { get; set; }
}

public class UpdateVaccinationRecordDTO
{
    public string? AdverseReaction { get; set; }
}

public class VaccinationRecordResponseDTO
{
    public int RecordId { get; set; }
    public int AppointmentId { get; set; }
    public int ChildId { get; set; }
    public string ChildName { get; set; }
    public int VaccineId { get; set; }
    public string VaccineName { get; set; }
    public DateOnly VaccinationDate { get; set; }
    public string? AdverseReaction { get; set; }
    public int StaffId { get; set; }
    public string StaffName { get; set; }
    public DateTime CreatedAt { get; set; }
} 