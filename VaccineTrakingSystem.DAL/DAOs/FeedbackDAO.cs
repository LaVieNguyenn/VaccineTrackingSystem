using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs;

public class FeedbackDAO : GenericDAO<Feedback>, IFeedbackDAO
{
    public FeedbackDAO(VaccineTrackingSystemDbContext context) : base(context)
    {
    }

    public async Task<List<Feedback>> GetCustomerFeedbacksAsync(int customerId)
    {
        return await _context.Feedbacks
            .Include(f => f.Appointment)
            .Include(f => f.Customer)
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
        return await _context.Feedbacks
            .AverageAsync(f => f.Rating);
    }

    public override async Task<Feedback> GetByIdAsync(int id)
    {
        return await _context.Feedbacks
            .Include(f => f.Appointment)
            .Include(f => f.Customer)
            .FirstOrDefaultAsync(f => f.FeedbackId == id);
    }
} 