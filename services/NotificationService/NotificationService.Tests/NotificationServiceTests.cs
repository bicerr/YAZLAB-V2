using NotificationService.Application.Repositories;
using NotificationService.Application.Services;
using NotificationService.Application.DTOs;
using NotificationService.Domain.Entities;

namespace NotificationService.Tests;

public class NotificationServiceTests
{
    [Fact]
    public async Task CreateNotification_ShouldReturnSuccess_WhenValid()
    {
        INotificationRepository repository = new FakeNotificationRepository();
        INotificationService service = new FakeNotificationService(repository);
        var result = await service.CreateAsync(new NotificationDto("user-1", "Siparişiniz onaylandı.", "Order"));
        Assert.True(result.Success);
    }

    [Fact]
    public async Task GetByUserId_ShouldReturnEmpty_WhenNoNotifications()
    {
        INotificationRepository repository = new FakeNotificationRepository();
        INotificationService service = new FakeNotificationService(repository);
        var result = await service.GetByUserIdAsync("user-1");
        Assert.Empty(result);
    }

    [Fact]
    public async Task MarkAsRead_ShouldReturnFalse_WhenNotFound()
    {
        INotificationRepository repository = new FakeNotificationRepository();
        INotificationService service = new FakeNotificationService(repository);
        var result = await service.MarkAsReadAsync("nonexistent-id");
        Assert.False(result);
    }

    [Fact]
public async Task GetUnreadByUserId_ShouldReturnUnread_WhenCalled()
{
    INotificationRepository repository = new FakeNotificationRepository();
    INotificationService service = new FakeNotificationService(repository);
    await service.CreateAsync(new NotificationDto("user-1", "Mesaj 1", "Order"));
    var result = await service.GetUnreadByUserIdAsync("user-1");
    Assert.Single(result);
}

[Fact]
public async Task GetUnreadCount_ShouldReturnCorrectCount_WhenCalled()
{
    INotificationRepository repository = new FakeNotificationRepository();
    INotificationService service = new FakeNotificationService(repository);
    await service.CreateAsync(new NotificationDto("user-1", "Mesaj 1", "Order"));
    await service.CreateAsync(new NotificationDto("user-1", "Mesaj 2", "Payment"));
    var result = await service.GetUnreadCountAsync("user-1");
    Assert.Equal(2, result);
}
}

public class FakeNotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public FakeNotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Notification>> GetByUserIdAsync(string userId)
        => await _repository.GetByUserIdAsync(userId);

    public async Task<NotificationResult> CreateAsync(NotificationDto dto)
    {
        var notification = new Notification(dto.UserId, dto.Message, dto.Type);
        await _repository.CreateAsync(notification);
        return new NotificationResult(true, null);
    }


    public async Task<List<Notification>> GetAllAsync()
    => await _repository.GetAllAsync();

public async Task<List<Notification>> GetUnreadByUserIdAsync(string userId)
    => await _repository.GetUnreadByUserIdAsync(userId);

public async Task<int> GetUnreadCountAsync(string userId)
    => await _repository.GetUnreadCountAsync(userId);

    public async Task<bool> MarkAsReadAsync(string id)
        => await _repository.MarkAsReadAsync(id);

    public async Task<bool> DeleteAsync(string id)
        => await _repository.DeleteAsync(id);
}