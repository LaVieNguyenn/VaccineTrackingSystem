namespace VaccineTrakingSystem.BLL.DTOs;

public class CreatePaymentDTO
{
    public int AppointmentId { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public byte PaymentMethod { get; set; }
}

public class UpdatePaymentStatusDTO
{
    public byte PaymentStatus { get; set; }
}

public class PaymentResponseDTO
{
    public int PaymentId { get; set; }
    public int AppointmentId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public byte PaymentMethod { get; set; }
    public string PaymentMethodName { get; set; }
    public byte PaymentStatus { get; set; }
    public string PaymentStatusName { get; set; }
    public DateTime CreatedAt { get; set; }
} 