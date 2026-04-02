namespace NotificationService.Domain.Entities;

public class Notification
{
    public string Id { get; private set; }
    public string UserId { get; private set; }
    public string Message { get; private set; }
    public string Type { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Notification(string userId, string message, string type)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        Message = message;
        Type = type;
        IsRead = false;
        CreatedAt = DateTime.UtcNow;
    }
}