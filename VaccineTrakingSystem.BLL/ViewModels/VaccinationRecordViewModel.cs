using System.ComponentModel.DataAnnotations;

namespace VaccineTrakingSystem.BLL.ViewModels;

public class VaccinationRecordViewModel
{
    public int RecordId { get; set; }
    
    [Display(Name = "Lịch hẹn")]
    public int AppointmentId { get; set; }
    
    [Display(Name = "Trẻ em")]
    public string ChildName { get; set; }
    
    [Display(Name = "Vắc xin")]
    public string VaccineName { get; set; }
    
    [Display(Name = "Ngày tiêm")]
    public DateOnly VaccinationDate { get; set; }
    
    [Display(Name = "Phản ứng phụ")]
    public string? AdverseReaction { get; set; }
    
    [Display(Name = "Nhân viên tiêm")]
    public string StaffName { get; set; }
    
    [Display(Name = "Ngày tạo")]
    public DateTime CreatedAt { get; set; }
}

public class VaccinationRecordListViewModel
{
    public List<VaccinationRecordViewModel> Records { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    
    [Display(Name = "Tổng số mũi tiêm")]
    public int TotalVaccinations { get; set; }
    
    [Display(Name = "Số mũi tiêm trong tháng")]
    public int MonthlyVaccinations { get; set; }
} 