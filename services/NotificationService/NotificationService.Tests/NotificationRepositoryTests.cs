using NotificationService.Application.Repositories;
using NotificationService.Domain.Entities;

namespace NotificationService.Tests;

public class NotificationRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmpty_WhenNoNotifications()
    {
        INotificationRepository repository = new FakeNotificationRepository();
        var result = await repository.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddNotification_WhenCalled()
    {
        INotificationRepository repository = new FakeNotificationRepository();
        var notification = new Notification("user-1", "Test mesajı", "Order");
        await repository.CreateAsync(notification);
        var result = await repository.GetAllAsync();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnNotifications_WhenExists()
    {
        INotificationRepository repository = new FakeNotificationRepository();
        var notification = new Notification("user-1", "Test mesajı", "Order");
        await repository.CreateAsync(notification);
        var result = await repository.GetByUserIdAsync("user-1");
        Assert.Single(result);
    }

    [Fact]
    public async Task MarkAsReadAsync_ShouldReturnTrue_WhenNotificationExists()
    {
        INotificationRepository repository = new FakeNotificationRepository();
        var notification = new Notification("user-1", "Test mesajı", "Order");
        await repository.CreateAsync(notification);
        var result = await repository.MarkAsReadAsync(notification.Id);
        Assert.True(result);
    }
}

public class FakeNotificationRepository : INotificationRepository
{
    private readonly List<Notification> _notifications = new();

    public Task<List<Notification>> GetAllAsync()
        => Task.FromResult(_notifications.ToList());

    public Task<List<Notification>> GetByUserIdAsync(string userId)
        => Task.FromResult(_notifications.Where(n => n.UserId == userId).ToList());

    public Task CreateAsync(Notification notification)
    {
        _notifications.Add(notification);
        return Task.CompletedTask;
    }

    public Task<bool> MarkAsReadAsync(string id)
    {
        var notification = _notifications.FirstOrDefault(n => n.Id == id);
        if (notification == null) return Task.FromResult(false);
        notification.MarkAsRead();
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(string id)
    {
        var notification = _notifications.FirstOrDefault(n => n.Id == id);
        if (notification == null) return Task.FromResult(false);
        _notifications.Remove(notification);
        return Task.FromResult(true);
    }
}