using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.ServicesService;

public class PaymentService : IPaymentService
{
    private readonly VaccineTrackingSystemDbContext _context;

    public PaymentService(VaccineTrackingSystemDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> CreatePaymentAsync(int appointmentId, int customerId, decimal amount, byte paymentMethod)
    {
        var payment = new Payment
        {
            AppointmentId = appointmentId,
            CustomerId = customerId,
            Amount = amount,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = paymentMethod,
            PaymentStatus = 0, // Pending
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> GetPaymentAsync(int paymentId)
    {
        return await _context.Payments
            .Include(p => p.Appointment)
            .Include(p => p.Customer)
            .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
    }

    public async Task<List<Payment>> GetCustomerPaymentsAsync(int customerId)
    {
        return await _context.Payments
            .Include(p => p.Appointment)
            .Where(p => p.CustomerId == customerId)
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

    public async Task<List<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Payments
            .Include(p => p.Appointment)
            .Include(p => p.Customer)
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }
} 