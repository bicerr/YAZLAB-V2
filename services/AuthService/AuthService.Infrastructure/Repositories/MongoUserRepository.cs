using AuthService.Application.Repositories;
using AuthService.Domain.Entities;
using MongoDB.Driver;

namespace AuthService.Infrastructure.Repositories;

public class MongoUserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public MongoUserRepository(IMongoCollection<User> users)
    {
        _users = users;
    }

    public async Task<User?> GetByEmailAsync(string email)
        => await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

    public async Task CreateAsync(User user)
        => await _users.InsertOneAsync(user);

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        var count = await _users.CountDocumentsAsync(u => u.Email == email);
        return count > 0;
    }
}