using AuthService.Application.Repositories;
using AuthService.Domain.Entities;

namespace AuthService.Tests;

public class UserRepositoryTests
{
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenUserNotFound()
    {
        IUserRepository repository = new FakeUserRepository();
        var result = await repository.GetByEmailAsync("notfound@test.com");
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddUser_WhenCalled()
    {
        IUserRepository repository = new FakeUserRepository();
        var user = new User("test@test.com", "hashedPassword", "user");
        await repository.CreateAsync(user);
        var result = await repository.GetByEmailAsync("test@test.com");
        Assert.NotNull(result);
    }
}

public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task<User?> GetByEmailAsync(string email)
        => Task.FromResult(_users.FirstOrDefault(u => u.Email == email));

    public Task CreateAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
}