using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTOs;
using NotificationService.Application.Services;
using Prometheus;

namespace NotificationService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private static readonly Counter NotificationsCreatedCounter = Metrics.CreateCounter(
        "notifications_created_total", "Bildirim oluşturma istekleri", new CounterConfiguration { LabelNames = new[] { "result" } });
    private static readonly Counter NotificationsReadCounter = Metrics.CreateCounter(
        "notifications_marked_read_total", "Okundu olarak işaretlenen bildirimler");
    private static readonly Counter NotificationsDeletedCounter = Metrics.CreateCounter(
        "notifications_deleted_total", "Silinen bildirimler", new CounterConfiguration { LabelNames = new[] { "result" } });

    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notifications = await _notificationService.GetAllAsync();
        return Ok(notifications);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var notifications = await _notificationService.GetByUserIdAsync(userId);
        return Ok(notifications);
    }

    [HttpGet("user/{userId}/unread")]
    public async Task<IActionResult> GetUnreadByUserId(string userId)
    {
        var notifications = await _notificationService.GetUnreadByUserIdAsync(userId);
        return Ok(notifications);
    }

    [HttpGet("user/{userId}/unread/count")]
    public async Task<IActionResult> GetUnreadCount(string userId)
    {
        var count = await _notificationService.GetUnreadCountAsync(userId);
        return Ok(new { count });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NotificationDto dto)
    {
        var result = await _notificationService.CreateAsync(dto);
        if (!result.Success)
        {
            NotificationsCreatedCounter.WithLabels("failure").Inc();
            return BadRequest(result.ErrorMessage);
        }
        NotificationsCreatedCounter.WithLabels("success").Inc();
        return Created("", result);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        var result = await _notificationService.MarkAsReadAsync(id);
        if (!result) return NotFound("Bildirim bulunamadı.");
        NotificationsReadCounter.Inc();
        return Ok("Bildirim okundu olarak işaretlendi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _notificationService.DeleteAsync(id);
        if (!result)
        {
            NotificationsDeletedCounter.WithLabels("failure").Inc();
            return NotFound("Bildirim bulunamadı.");
        }
        NotificationsDeletedCounter.WithLabels("success").Inc();
        return Ok("Bildirim silindi.");
    }
}