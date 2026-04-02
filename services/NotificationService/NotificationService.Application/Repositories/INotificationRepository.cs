using NotificationService.Domain.Entities;

namespace NotificationService.Application.Repositories;

public interface INotificationRepository
{
    Task<List<Notification>> GetAllAsync();
    Task<List<Notification>> GetByUserIdAsync(string userId);
    Task CreateAsync(Notification notification);
    Task<bool> MarkAsReadAsync(string id);
    Task<bool> DeleteAsync(string id);
}