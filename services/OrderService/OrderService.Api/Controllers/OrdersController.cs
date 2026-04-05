using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Services;
using Prometheus;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly Counter OrdersCreatedCounter = Metrics.CreateCounter(
        "orders_created_total", "Sipariş oluşturma istekleri", new CounterConfiguration { LabelNames = new[] { "result" } });
    private static readonly Counter OrdersStatusUpdatedCounter = Metrics.CreateCounter(
        "orders_status_updated_total", "Sipariş durum güncellemeleri", new CounterConfiguration { LabelNames = new[] { "status" } });
    private static readonly Counter OrdersDeletedCounter = Metrics.CreateCounter(
        "orders_deleted_total", "Sipariş silme istekleri", new CounterConfiguration { LabelNames = new[] { "result" } });

    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound("Sipariş bulunamadı.");
        return Ok(order);
    }

    [HttpGet("customer/{email}")]
    public async Task<IActionResult> GetByCustomerEmail(string email)
    {
        var orders = await _orderService.GetByCustomerEmailAsync(email);
        return Ok(orders);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var orders = await _orderService.GetByStatusAsync(status);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderDto dto)
    {
        var result = await _orderService.CreateAsync(dto);
        if (!result.Success)
        {
            OrdersCreatedCounter.WithLabels("failure").Inc();
            return BadRequest(result.ErrorMessage);
        }
        OrdersCreatedCounter.WithLabels("success").Inc();
        return Created("", result);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] string status)
    {
        var result = await _orderService.UpdateStatusAsync(id, status);
        if (!result) return NotFound("Sipariş bulunamadı.");
        OrdersStatusUpdatedCounter.WithLabels(status).Inc();
        return Ok("Sipariş güncellendi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _orderService.DeleteAsync(id);
        if (!result)
        {
            OrdersDeletedCounter.WithLabels("failure").Inc();
            return NotFound("Sipariş bulunamadı.");
        }
        OrdersDeletedCounter.WithLabels("success").Inc();
        return Ok("Sipariş silindi.");
    }
}