using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.ServicesService;

public class FeedbackService : IFeedbackService
{
    private readonly VaccineTrackingSystemDbContext _context;

    public FeedbackService(VaccineTrackingSystemDbContext context)
    {
        _context = context;
    }

    public async Task<Feedback> CreateFeedbackAsync(int? appointmentId, int customerId, int rating, string? comments)
    {
        var feedback = new Feedback
        {
            AppointmentId = appointmentId,
            CustomerId = customerId,
            Rating = rating,
            Comments = comments,
            FeedbackDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();
        return feedback;
    }

    public async Task<Feedback> GetFeedbackAsync(int feedbackId)
    {
        return await _context.Feedbacks
            .Include(f => f.Appointment)
            .Include(f => f.Customer)
            .FirstOrDefaultAsync(f => f.FeedbackId == feedbackId);
    }

    public async Task<List<Feedback>> GetCustomerFeedbacksAsync(int customerId)
    {
        return await _context.Feedbacks
            .Include(f => f.Appointment)
            .Where(f => f.CustomerId == customerId)
            .OrderByDescending(f => f.FeedbackDate)
            .ToListAsync();
    }

    public async Task<List<Feedback>> GetAppointmentFeedbacksAsync(int appointmentId)
    {
        return await _context.Feedbacks
            .Include(f => f.Customer)
            .Where(f => f.AppointmentId == appointmentId)
            .OrderByDescending(f => f.FeedbackDate)
            .ToListAsync();
    }

    public async Task<double> GetAverageRatingAsync()
    {
        var averageRating = await _context.Feedbacks
            .AverageAsync(f => f.Rating);
        return averageRating;
    }

    public async Task<bool> DeleteFeedbackAsync(int feedbackId)
    {
        var feedback = await _context.Feedbacks.FindAsync(feedbackId);
        if (feedback == null) return false;

        _context.Feedbacks.Remove(feedback);
        await _context.SaveChangesAsync();
        return true;
    }
} 