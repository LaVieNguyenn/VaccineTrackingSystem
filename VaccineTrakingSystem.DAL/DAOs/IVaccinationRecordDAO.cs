using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs;

public interface IVaccinationRecordDAO : IGenericDAO<VaccinationRecord>
{
    Task<List<VaccinationRecord>> GetChildVaccinationRecordsAsync(int childId);
    Task<List<VaccinationRecord>> GetStaffVaccinationRecordsAsync(int staffId, DateOnly date);
    Task<bool> UpdateAdverseReactionAsync(int recordId, string? adverseReaction);
} 