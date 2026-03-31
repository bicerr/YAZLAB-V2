using AuthService.Domain.Entities;

namespace AuthService.Application.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task CreateAsync(User user);
}