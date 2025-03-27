using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.ServicesService;

public interface IVaccinationRecordService
{
    Task<VaccinationRecord> CreateVaccinationRecordAsync(int appointmentId, int childId, int vaccineId, DateOnly vaccinationDate, string? adverseReaction, int staffId);
    Task<VaccinationRecord> GetVaccinationRecordAsync(int recordId);
    Task<List<VaccinationRecord>> GetChildVaccinationRecordsAsync(int childId);
    Task<bool> UpdateVaccinationRecordAsync(int recordId, string? adverseReaction);
    Task<List<VaccinationRecord>> GetStaffVaccinationRecordsAsync(int staffId, DateOnly date);
} 