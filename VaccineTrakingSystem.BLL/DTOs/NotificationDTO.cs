namespace VaccineTrakingSystem.BLL.DTOs;

public class CreateNotificationDTO
{
    public int UserId { get; set; }
    public int? ChildId { get; set; }
    public int? AppointmentId { get; set; }
    public string Message { get; set; }
    public DateTime NotificationDate { get; set; }
}

public class NotificationResponseDTO
{
    public int NotificationId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int? ChildId { get; set; }
    public string ChildName { get; set; }
    public int? AppointmentId { get; set; }
    public string Message { get; set; }
    public DateTime NotificationDate { get; set; }
    public bool IsSent { get; set; }
    public DateTime CreatedAt { get; set; }
} 