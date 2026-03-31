namespace AuthService.Domain.Entities;

public class User
{
    public string Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User(string email, string passwordHash, string role)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);
        ArgumentException.ThrowIfNullOrEmpty(passwordHash);
        ArgumentException.ThrowIfNullOrEmpty(role);

        Id = Guid.NewGuid().ToString();
        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }
}