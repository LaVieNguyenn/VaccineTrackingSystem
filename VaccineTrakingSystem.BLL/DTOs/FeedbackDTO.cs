namespace VaccineTrakingSystem.BLL.DTOs;

public class CreateFeedbackDTO
{
    public int? AppointmentId { get; set; }
    public int CustomerId { get; set; }
    public int Rating { get; set; }
    public string? Comments { get; set; }
}

public class FeedbackResponseDTO
{
    public int FeedbackId { get; set; }
    public int? AppointmentId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int Rating { get; set; }
    public string? Comments { get; set; }
    public DateTime FeedbackDate { get; set; }
    public DateTime CreatedAt { get; set; }
} 