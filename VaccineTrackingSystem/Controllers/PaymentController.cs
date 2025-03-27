using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.DTOs;
using VaccineTrakingSystem.BLL.ServicesService;

namespace VaccineTrackingSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<ActionResult<PaymentResponseDTO>> CreatePayment([FromBody] CreatePaymentDTO dto)
    {
        var payment = await _paymentService.CreatePaymentAsync(
            dto.AppointmentId,
            dto.CustomerId,
            dto.Amount,
            dto.PaymentMethod
        );

        return Ok(new PaymentResponseDTO
        {
            PaymentId = payment.PaymentId,
            AppointmentId = payment.AppointmentId,
            CustomerId = payment.CustomerId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod,
            PaymentStatus = payment.PaymentStatus,
            CreatedAt = payment.CreatedAt
        });
    }

    [HttpGet("{paymentId}")]
    public async Task<ActionResult<PaymentResponseDTO>> GetPayment(int paymentId)
    {
        var payment = await _paymentService.GetPaymentAsync(paymentId);
        if (payment == null) return NotFound();

        return Ok(new PaymentResponseDTO
        {
            PaymentId = payment.PaymentId,
            AppointmentId = payment.AppointmentId,
            CustomerId = payment.CustomerId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod,
            PaymentStatus = payment.PaymentStatus,
            CreatedAt = payment.CreatedAt
        });
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<List<PaymentResponseDTO>>> GetCustomerPayments(int customerId)
    {
        var payments = await _paymentService.GetCustomerPaymentsAsync(customerId);
        return Ok(payments.Select(p => new PaymentResponseDTO
        {
            PaymentId = p.PaymentId,
            AppointmentId = p.AppointmentId,
            CustomerId = p.CustomerId,
            Amount = p.Amount,
            PaymentDate = p.PaymentDate,
            PaymentMethod = p.PaymentMethod,
            PaymentStatus = p.PaymentStatus,
            CreatedAt = p.CreatedAt
        }));
    }

    [HttpPut("{paymentId}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdatePaymentStatus(int paymentId, [FromBody] UpdatePaymentStatusDTO dto)
    {
        var result = await _paymentService.UpdatePaymentStatusAsync(paymentId, dto.PaymentStatus);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpGet("date-range")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<PaymentResponseDTO>>> GetPaymentsByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var payments = await _paymentService.GetPaymentsByDateRangeAsync(startDate, endDate);
        return Ok(payments.Select(p => new PaymentResponseDTO
        {
            PaymentId = p.PaymentId,
            AppointmentId = p.AppointmentId,
            CustomerId = p.CustomerId,
            Amount = p.Amount,
            PaymentDate = p.PaymentDate,
            PaymentMethod = p.PaymentMethod,
            PaymentStatus = p.PaymentStatus,
            CreatedAt = p.CreatedAt
        }));
    }
} 