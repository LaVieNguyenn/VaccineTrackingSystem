using System.ComponentModel.DataAnnotations;

namespace VaccineTrakingSystem.BLL.ViewModels;

public class NotificationViewModel
{
    public int NotificationId { get; set; }
    
    [Display(Name = "Người nhận")]
    public string UserName { get; set; }
    
    [Display(Name = "Trẻ em")]
    public string? ChildName { get; set; }
    
    [Display(Name = "Lịch hẹn")]
    public int? AppointmentId { get; set; }
    
    [Display(Name = "Nội dung")]
    public string Message { get; set; }
    
    [Display(Name = "Ngày thông báo")]
    public DateTime NotificationDate { get; set; }
    
    [Display(Name = "Trạng thái")]
    public bool IsSent { get; set; }
    
    [Display(Name = "Ngày tạo")]
    public DateTime CreatedAt { get; set; }
}

public class NotificationListViewModel
{
    public List<NotificationViewModel> Notifications { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
} 