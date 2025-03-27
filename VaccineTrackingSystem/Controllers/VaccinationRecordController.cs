using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.DTOs;
using VaccineTrakingSystem.BLL.ServicesService;

namespace VaccineTrackingSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VaccinationRecordController : ControllerBase
{
    private readonly IVaccinationRecordService _vaccinationRecordService;

    public VaccinationRecordController(IVaccinationRecordService vaccinationRecordService)
    {
        _vaccinationRecordService = vaccinationRecordService;
    }

    [HttpPost]
    [Authorize(Roles = "Staff")]
    public async Task<ActionResult<VaccinationRecordResponseDTO>> CreateVaccinationRecord([FromBody] CreateVaccinationRecordDTO dto)
    {
        var record = await _vaccinationRecordService.CreateVaccinationRecordAsync(
            dto.AppointmentId,
            dto.ChildId,
            dto.VaccineId,
            dto.VaccinationDate,
            dto.AdverseReaction,
            dto.StaffId
        );

        return Ok(new VaccinationRecordResponseDTO
        {
            RecordId = record.RecordId,
            AppointmentId = record.AppointmentId,
            ChildId = record.ChildId,
            VaccineId = record.VaccineId,
            VaccinationDate = record.VaccinationDate,
            AdverseReaction = record.AdverseReaction,
            StaffId = record.StaffId,
            CreatedAt = record.CreatedAt
        });
    }

    [HttpGet("{recordId}")]
    public async Task<ActionResult<VaccinationRecordResponseDTO>> GetVaccinationRecord(int recordId)
    {
        var record = await _vaccinationRecordService.GetVaccinationRecordAsync(recordId);
        if (record == null) return NotFound();

        return Ok(new VaccinationRecordResponseDTO
        {
            RecordId = record.RecordId,
            AppointmentId = record.AppointmentId,
            ChildId = record.ChildId,
            VaccineId = record.VaccineId,
            VaccinationDate = record.VaccinationDate,
            AdverseReaction = record.AdverseReaction,
            StaffId = record.StaffId,
            CreatedAt = record.CreatedAt
        });
    }

    [HttpGet("child/{childId}")]
    public async Task<ActionResult<List<VaccinationRecordResponseDTO>>> GetChildVaccinationRecords(int childId)
    {
        var records = await _vaccinationRecordService.GetChildVaccinationRecordsAsync(childId);
        return Ok(records.Select(r => new VaccinationRecordResponseDTO
        {
            RecordId = r.RecordId,
            AppointmentId = r.AppointmentId,
            ChildId = r.ChildId,
            VaccineId = r.VaccineId,
            VaccinationDate = r.VaccinationDate,
            AdverseReaction = r.AdverseReaction,
            StaffId = r.StaffId,
            CreatedAt = r.CreatedAt
        }));
    }

    [HttpPut("{recordId}")]
    [Authorize(Roles = "Staff")]
    public async Task<ActionResult> UpdateVaccinationRecord(int recordId, [FromBody] UpdateVaccinationRecordDTO dto)
    {
        var result = await _vaccinationRecordService.UpdateVaccinationRecordAsync(recordId, dto.AdverseReaction);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpGet("staff/{staffId}/date/{date}")]
    [Authorize(Roles = "Staff")]
    public async Task<ActionResult<List<VaccinationRecordResponseDTO>>> GetStaffVaccinationRecords(int staffId, DateOnly date)
    {
        var records = await _vaccinationRecordService.GetStaffVaccinationRecordsAsync(staffId, date);
        return Ok(records.Select(r => new VaccinationRecordResponseDTO
        {
            RecordId = r.RecordId,
            AppointmentId = r.AppointmentId,
            ChildId = r.ChildId,
            VaccineId = r.VaccineId,
            VaccinationDate = r.VaccinationDate,
            AdverseReaction = r.AdverseReaction,
            StaffId = r.StaffId,
            CreatedAt = r.CreatedAt
        }));
    }
} 