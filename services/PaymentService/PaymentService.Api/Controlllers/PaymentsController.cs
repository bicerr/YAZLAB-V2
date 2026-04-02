using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTOs;
using PaymentService.Application.Services;

namespace PaymentService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
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
        if (!result.Success) return BadRequest(result.ErrorMessage);
        return Created("", result);
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(string id)
    {
        var result = await _paymentService.CompleteAsync(id);
        if (!result) return NotFound("Ödeme bulunamadı.");
        return Ok("Ödeme tamamlandı.");
    }

    [HttpPut("{id}/fail")]
    public async Task<IActionResult> Fail(string id)
    {
        var result = await _paymentService.FailAsync(id);
        if (!result) return NotFound("Ödeme bulunamadı.");
        return Ok("Ödeme başarısız olarak işaretlendi.");
    }
}