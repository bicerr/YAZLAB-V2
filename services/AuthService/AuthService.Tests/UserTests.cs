using AuthService.Domain.Entities;

namespace AuthService.Tests;

public class UserTests
{
    [Fact]
    public void User_ShouldHaveEmail_WhenCreated()
    {
        var user = new User("test@test.com", "hashedPassword", "user");
        Assert.Equal("test@test.com", user.Email);
    }

    [Fact]
    public void User_ShouldHaveRole_WhenCreated()
    {
        var user = new User("test@test.com", "hashedPassword", "user");
        Assert.Equal("user", user.Role);
    }

    [Fact]
    public void User_ShouldHavePasswordHash_WhenCreated()
    {
        var user = new User("test@test.com", "hashedPassword", "user");
        Assert.Equal("hashedPassword", user.PasswordHash);
    }
}