using System.ComponentModel.DataAnnotations;

namespace VaccineTrakingSystem.BLL.DTOs.Validation;

public class CreateNotificationDTO
{
    [Required(ErrorMessage = "UserId là bắt buộc")]
    public int UserId { get; set; }

    public int? ChildId { get; set; }

    public int? AppointmentId { get; set; }

    [Required(ErrorMessage = "Message là bắt buộc")]
    [StringLength(500, ErrorMessage = "Message không được vượt quá 500 ký tự")]
    public string Message { get; set; }

    [Required(ErrorMessage = "NotificationDate là bắt buộc")]
    public DateTime NotificationDate { get; set; }
} 