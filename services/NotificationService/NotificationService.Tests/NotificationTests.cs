using NotificationService.Domain.Entities;

namespace NotificationService.Tests;

public class NotificationTests
{
    [Fact]
    public void Notification_ShouldHaveUserId_WhenCreated()
    {
        var notification = new Notification("user-1", "Siparişiniz onaylandı.", "Order");
        Assert.Equal("user-1", notification.UserId);
    }

    [Fact]
    public void Notification_ShouldHaveMessage_WhenCreated()
    {
        var notification = new Notification("user-1", "Siparişiniz onaylandı.", "Order");
        Assert.Equal("Siparişiniz onaylandı.", notification.Message);
    }

    [Fact]
    public void Notification_ShouldBeUnread_WhenCreated()
    {
        var notification = new Notification("user-1", "Siparişiniz onaylandı.", "Order");
        Assert.False(notification.IsRead);
    }

    [Fact]
    public void Notification_ShouldHaveType_WhenCreated()
    {
        var notification = new Notification("user-1", "Siparişiniz onaylandı.", "Order");
        Assert.Equal("Order", notification.Type);
    }
}