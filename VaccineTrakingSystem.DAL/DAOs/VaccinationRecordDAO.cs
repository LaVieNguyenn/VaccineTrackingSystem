using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs;

public class VaccinationRecordDAO : GenericDAO<VaccinationRecord>, IVaccinationRecordDAO
{
    public VaccinationRecordDAO(VaccineTrackingSystemDbContext context) : base(context)
    {
    }

    public async Task<List<VaccinationRecord>> GetChildVaccinationRecordsAsync(int childId)
    {
        return await _context.VaccinationRecords
            .Include(v => v.Vaccine)
            .Include(v => v.Staff)
            .Include(v => v.Appointment)
            .Where(v => v.ChildId == childId)
            .OrderByDescending(v => v.VaccinationDate)
            .ToListAsync();
    }

    public async Task<List<VaccinationRecord>> GetStaffVaccinationRecordsAsync(int staffId, DateOnly date)
    {
        return await _context.VaccinationRecords
            .Include(v => v.Child)
            .Include(v => v.Vaccine)
            .Include(v => v.Appointment)
            .Where(v => v.StaffId == staffId && v.VaccinationDate == date)
            .OrderBy(v => v.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> UpdateAdverseReactionAsync(int recordId, string? adverseReaction)
    {
        var record = await _context.VaccinationRecords.FindAsync(recordId);
        if (record == null) return false;

        record.AdverseReaction = adverseReaction;
        record.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public override async Task<VaccinationRecord> GetByIdAsync(int id)
    {
        return await _context.VaccinationRecords
            .Include(v => v.Appointment)
            .Include(v => v.Child)
            .Include(v => v.Vaccine)
            .Include(v => v.Staff)
            .FirstOrDefaultAsync(v => v.RecordId == id);
    }
} 