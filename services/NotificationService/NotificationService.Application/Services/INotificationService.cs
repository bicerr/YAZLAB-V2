using NotificationService.Application.DTOs;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Services;

public interface INotificationService
{
    Task<List<Notification>> GetByUserIdAsync(string userId);
    Task<NotificationResult> CreateAsync(NotificationDto dto);
    Task<bool> MarkAsReadAsync(string id);
    Task<bool> DeleteAsync(string id);
}