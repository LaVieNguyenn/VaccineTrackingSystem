using System.ComponentModel.DataAnnotations;
using VaccineTrakingSystem.BLL.Enums;

namespace VaccineTrakingSystem.BLL.DTOs.Validation;

public class CreatePaymentDTO
{
    [Required(ErrorMessage = "AppointmentId là bắt buộc")]
    public int AppointmentId { get; set; }

    [Required(ErrorMessage = "CustomerId là bắt buộc")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Amount là bắt buộc")]
    [Range(0, 999999999.99, ErrorMessage = "Amount phải lớn hơn 0 và nhỏ hơn 1 tỷ")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "PaymentMethod là bắt buộc")]
    [EnumDataType(typeof(PaymentMethod), ErrorMessage = "PaymentMethod không hợp lệ")]
    public byte PaymentMethod { get; set; }
}

public class UpdatePaymentStatusDTO
{
    [Required(ErrorMessage = "PaymentStatus là bắt buộc")]
    [EnumDataType(typeof(PaymentStatus), ErrorMessage = "PaymentStatus không hợp lệ")]
    public byte PaymentStatus { get; set; }
} 