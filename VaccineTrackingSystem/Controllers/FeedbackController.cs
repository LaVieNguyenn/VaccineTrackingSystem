using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.DTOs;
using VaccineTrakingSystem.BLL.ServicesService;

namespace VaccineTrackingSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpPost]
    public async Task<ActionResult<FeedbackResponseDTO>> CreateFeedback([FromBody] CreateFeedbackDTO dto)
    {
        var feedback = await _feedbackService.CreateFeedbackAsync(
            dto.AppointmentId,
            dto.CustomerId,
            dto.Rating,
            dto.Comments
        );

        return Ok(new FeedbackResponseDTO
        {
            FeedbackId = feedback.FeedbackId,
            AppointmentId = feedback.AppointmentId,
            CustomerId = feedback.CustomerId,
            Rating = feedback.Rating,
            Comments = feedback.Comments,
            FeedbackDate = feedback.FeedbackDate,
            CreatedAt = feedback.CreatedAt
        });
    }

    [HttpGet("{feedbackId}")]
    public async Task<ActionResult<FeedbackResponseDTO>> GetFeedback(int feedbackId)
    {
        var feedback = await _feedbackService.GetFeedbackAsync(feedbackId);
        if (feedback == null) return NotFound();

        return Ok(new FeedbackResponseDTO
        {
            FeedbackId = feedback.FeedbackId,
            AppointmentId = feedback.AppointmentId,
            CustomerId = feedback.CustomerId,
            Rating = feedback.Rating,
            Comments = feedback.Comments,
            FeedbackDate = feedback.FeedbackDate,
            CreatedAt = feedback.CreatedAt
        });
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<List<FeedbackResponseDTO>>> GetCustomerFeedbacks(int customerId)
    {
        var feedbacks = await _feedbackService.GetCustomerFeedbacksAsync(customerId);
        return Ok(feedbacks.Select(f => new FeedbackResponseDTO
        {
            FeedbackId = f.FeedbackId,
            AppointmentId = f.AppointmentId,
            CustomerId = f.CustomerId,
            Rating = f.Rating,
            Comments = f.Comments,
            FeedbackDate = f.FeedbackDate,
            CreatedAt = f.CreatedAt
        }));
    }

    [HttpGet("appointment/{appointmentId}")]
    public async Task<ActionResult<List<FeedbackResponseDTO>>> GetAppointmentFeedbacks(int appointmentId)
    {
        var feedbacks = await _feedbackService.GetAppointmentFeedbacksAsync(appointmentId);
        return Ok(feedbacks.Select(f => new FeedbackResponseDTO
        {
            FeedbackId = f.FeedbackId,
            AppointmentId = f.AppointmentId,
            CustomerId = f.CustomerId,
            Rating = f.Rating,
            Comments = f.Comments,
            FeedbackDate = f.FeedbackDate,
            CreatedAt = f.CreatedAt
        }));
    }

    [HttpGet("average-rating")]
    public async Task<ActionResult<double>> GetAverageRating()
    {
        var averageRating = await _feedbackService.GetAverageRatingAsync();
        return Ok(averageRating);
    }

    [HttpDelete("{feedbackId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteFeedback(int feedbackId)
    {
        var result = await _feedbackService.DeleteFeedbackAsync(feedbackId);
        if (!result) return NotFound();
        return Ok();
    }
} 