using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs;

public class PaymentDAO : GenericDAO<Payment>, IPaymentDAO
{
    public PaymentDAO(VaccineTrackingSystemDbContext context) : base(context)
    {
    }

    public async Task<List<Payment>> GetCustomerPaymentsAsync(int customerId)
    {
        return await _context.Payments
            .Include(p => p.Appointment)
            .Include(p => p.Customer)
            .Where(p => p.CustomerId == customerId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<List<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Payments
            .Include(p => p.Appointment)
            .Include(p => p.Customer)
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<bool> UpdatePaymentStatusAsync(int paymentId, byte paymentStatus)
    {
        var payment = await _context.Payments.FindAsync(paymentId);
        if (payment == null) return false;

        payment.PaymentStatus = paymentStatus;
        payment.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public override async Task<Payment> GetByIdAsync(int id)
    {
        return await _context.Payments
            .Include(p => p.Appointment)
            .Include(p => p.Customer)
            .FirstOrDefaultAsync(p => p.PaymentId == id);
    }
} 