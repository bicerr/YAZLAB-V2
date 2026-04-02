using NotificationService.Application.DTOs;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Services;

/// <summary>
/// Bildirim iş mantığı için servis sözleşmesi
/// </summary>
public interface INotificationService
{
    Task<List<Notification>> GetAllAsync();
    Task<List<Notification>> GetByUserIdAsync(string userId);
    Task<List<Notification>> GetUnreadByUserIdAsync(string userId);
    Task<int> GetUnreadCountAsync(string userId);
    Task<NotificationResult> CreateAsync(NotificationDto dto);
    Task<bool> MarkAsReadAsync(string id);
    Task<bool> DeleteAsync(string id);
}