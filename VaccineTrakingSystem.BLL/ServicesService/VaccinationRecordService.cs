using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.ServicesService;

public class VaccinationRecordService : IVaccinationRecordService
{
    private readonly VaccineTrackingSystemDbContext _context;

    public VaccinationRecordService(VaccineTrackingSystemDbContext context)
    {
        _context = context;
    }

    public async Task<VaccinationRecord> CreateVaccinationRecordAsync(int appointmentId, int childId, int vaccineId, DateOnly vaccinationDate, string? adverseReaction, int staffId)
    {
        var record = new VaccinationRecord
        {
            AppointmentId = appointmentId,
            ChildId = childId,
            VaccineId = vaccineId,
            VaccinationDate = vaccinationDate,
            AdverseReaction = adverseReaction,
            StaffId = staffId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.VaccinationRecords.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task<VaccinationRecord> GetVaccinationRecordAsync(int recordId)
    {
        return await _context.VaccinationRecords
            .Include(v => v.Appointment)
            .Include(v => v.Child)
            .Include(v => v.Vaccine)
            .Include(v => v.Staff)
            .FirstOrDefaultAsync(v => v.RecordId == recordId);
    }

    public async Task<List<VaccinationRecord>> GetChildVaccinationRecordsAsync(int childId)
    {
        return await _context.VaccinationRecords
            .Include(v => v.Vaccine)
            .Where(v => v.ChildId == childId)
            .OrderByDescending(v => v.VaccinationDate)
            .ToListAsync();
    }

    public async Task<bool> UpdateVaccinationRecordAsync(int recordId, string? adverseReaction)
    {
        var record = await _context.VaccinationRecords.FindAsync(recordId);
        if (record == null) return false;

        record.AdverseReaction = adverseReaction;
        record.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<VaccinationRecord>> GetStaffVaccinationRecordsAsync(int staffId, DateOnly date)
    {
        return await _context.VaccinationRecords
            .Include(v => v.Child)
            .Include(v => v.Vaccine)
            .Where(v => v.StaffId == staffId && v.VaccinationDate == date)
            .OrderBy(v => v.CreatedAt)
            .ToListAsync();
    }
} 