using NotificationService.Application.DTOs;
using NotificationService.Application.Repositories;
using NotificationService.Application.Services;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Services;

public class NotificationServiceImpl : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationServiceImpl(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Notification>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<List<Notification>> GetByUserIdAsync(string userId)
        => await _repository.GetByUserIdAsync(userId);

    public async Task<List<Notification>> GetUnreadByUserIdAsync(string userId)
        => await _repository.GetUnreadByUserIdAsync(userId);

    public async Task<int> GetUnreadCountAsync(string userId)
        => await _repository.GetUnreadCountAsync(userId);

    public async Task<NotificationResult> CreateAsync(NotificationDto dto)
    {
        var notification = new Notification(dto.UserId, dto.Message, dto.Type);
        await _repository.CreateAsync(notification);
        return new NotificationResult(true, null);
    }

    public async Task<bool> MarkAsReadAsync(string id)
        => await _repository.MarkAsReadAsync(id);

    public async Task<bool> DeleteAsync(string id)
        => await _repository.DeleteAsync(id);
}