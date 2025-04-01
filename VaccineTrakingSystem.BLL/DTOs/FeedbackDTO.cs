using System;

namespace VaccineTrakingSystem.BLL.DTOs
{
    public class FeedbackDTO
    {
        public int FeedbackId { get; set; }
        public int? AppointmentId { get; set; }
        public int CustomerId { get; set; }
        public int Rating { get; set; }
        public string? Comments { get; set; }
        public DateTime FeedbackDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 