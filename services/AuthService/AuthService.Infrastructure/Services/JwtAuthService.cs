using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Application.DTOs;
using AuthService.Application.Repositories;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.Services;

public class JwtAuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly string _jwtKey = "supersecretkey1234567890abcdefgh";

    public JwtAuthService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        var exists = await _repository.ExistsByEmailAsync(request.Email);
        if (exists) return new AuthResult(false, null, "Bu email zaten kayıtlı.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(request.Email, passwordHash, "user");
        await _repository.CreateAsync(user);
        return new AuthResult(true, null, null);
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var user = await _repository.GetByEmailAsync(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return new AuthResult(false, null, "Email veya şifre hatalı.");

        var token = GenerateToken(user);
        return new AuthResult(true, token, null);
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtKey);
            handler.ValidateToken(token.Replace("Bearer ", ""), new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out _);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}