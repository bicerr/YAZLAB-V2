namespace AuthService.Domain.Entities;

public class User
{
    public string Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }

    public User(string email, string passwordHash, string role)
    {
        Id = Guid.NewGuid().ToString();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
}