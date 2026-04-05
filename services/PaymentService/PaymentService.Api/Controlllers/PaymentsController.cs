using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTOs;
using PaymentService.Application.Services;
using Prometheus;

namespace PaymentService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private static readonly Counter PaymentsCreatedCounter = Metrics.CreateCounter(
        "payments_created_total", "Ödeme oluşturma istekleri", new CounterConfiguration { LabelNames = new[] { "result" } });
    private static readonly Counter PaymentsCompletedCounter = Metrics.CreateCounter(
        "payments_completed_total", "Tamamlanan ödemeler");
    private static readonly Counter PaymentsFailedCounter = Metrics.CreateCounter(
        "payments_failed_total", "Başarısız ödemeler");

    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var payments = await _paymentService.GetAllAsync();
        return Ok(payments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment == null) return NotFound("Ödeme bulunamadı.");
        return Ok(payment);
    }

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetByOrderId(string orderId)
    {
        var payment = await _paymentService.GetByOrderIdAsync(orderId);
        if (payment == null) return NotFound("Sipariş için ödeme bulunamadı.");
        return Ok(payment);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var payments = await _paymentService.GetByStatusAsync(status);
        return Ok(payments);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PaymentDto dto)
    {
        var result = await _paymentService.CreateAsync(dto);
        if (!result.Success)
        {
            PaymentsCreatedCounter.WithLabels("failure").Inc();
            return BadRequest(result.ErrorMessage);
        }
        PaymentsCreatedCounter.WithLabels("success").Inc();
        return Created("", result);
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(string id)
    {
        var result = await _paymentService.CompleteAsync(id);
        if (!result) return NotFound("Ödeme bulunamadı.");
        PaymentsCompletedCounter.Inc();
        return Ok("Ödeme tamamlandı.");
    }

    [HttpPut("{id}/fail")]
    public async Task<IActionResult> Fail(string id)
    {
        var result = await _paymentService.FailAsync(id);
        if (!result) return NotFound("Ödeme bulunamadı.");
        PaymentsFailedCounter.Inc();
        return Ok("Ödeme başarısız olarak işaretlendi.");
    }
}