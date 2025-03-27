using System.ComponentModel.DataAnnotations;

namespace VaccineTrakingSystem.BLL.DTOs.Validation;

public class CreateFeedbackDTO
{
    public int? AppointmentId { get; set; }

    [Required(ErrorMessage = "CustomerId là bắt buộc")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Rating là bắt buộc")]
    [Range(1, 5, ErrorMessage = "Rating phải từ 1 đến 5")]
    public int Rating { get; set; }

    [StringLength(1000, ErrorMessage = "Comments không được vượt quá 1000 ký tự")]
    public string? Comments { get; set; }
} 