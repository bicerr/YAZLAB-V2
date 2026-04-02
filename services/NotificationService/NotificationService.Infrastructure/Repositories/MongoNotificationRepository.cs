using MongoDB.Driver;
using NotificationService.Application.Repositories;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Repositories;

public class MongoNotificationRepository : INotificationRepository
{
    private readonly IMongoCollection<Notification> _notifications;

    public MongoNotificationRepository(IMongoCollection<Notification> notifications)
    {
        _notifications = notifications;
    }

    public async Task<List<Notification>> GetAllAsync()
        => await _notifications.Find(_ => true).SortByDescending(n => n.CreatedAt).ToListAsync();

    public async Task<List<Notification>> GetByUserIdAsync(string userId)
        => await _notifications.Find(n => n.UserId == userId).ToListAsync();

    public async Task<List<Notification>> GetUnreadByUserIdAsync(string userId)
        => await _notifications.Find(n => n.UserId == userId && !n.IsRead).ToListAsync();

    public async Task<Notification?> GetByIdAsync(string id)
        => await _notifications.Find(n => n.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Notification notification)
        => await _notifications.InsertOneAsync(notification);

    public async Task<bool> MarkAsReadAsync(string id)
    {
        var notification = await GetByIdAsync(id);
        if (notification == null) return false;
        notification.MarkAsRead();
        var result = await _notifications.ReplaceOneAsync(n => n.Id == id, notification);
        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _notifications.DeleteOneAsync(n => n.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        var count = await _notifications.CountDocumentsAsync(n => n.UserId == userId && !n.IsRead);
        return (int)count;
    }
}