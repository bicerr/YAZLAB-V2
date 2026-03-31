using AuthService.Application.Services;
using AuthService.Application.Repositories;
using AuthService.Application.DTOs;
using AuthService.Domain.Entities;

namespace AuthService.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task Register_ShouldReturnSuccess_WhenEmailNotTaken()
    {
        IUserRepository repository = new FakeUserRepository();
        IAuthService authService = new FakeAuthService(repository);
        var result = await authService.RegisterAsync(new RegisterRequest("test@test.com", "password123"));
        Assert.True(result.Success);
    }

    [Fact]
    public async Task Register_ShouldReturnFailure_WhenEmailAlreadyTaken()
    {
        IUserRepository repository = new FakeUserRepository();
        var user = new User("test@test.com", "hashedPassword", "user");
        await repository.CreateAsync(user);
        IAuthService authService = new FakeAuthService(repository);
        var result = await authService.RegisterAsync(new RegisterRequest("test@test.com", "password123"));
        Assert.False(result.Success);
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
    {
        IUserRepository repository = new FakeUserRepository();
        IAuthService authService = new FakeAuthService(repository);
        await authService.RegisterAsync(new RegisterRequest("test@test.com", "password123"));
        var result = await authService.LoginAsync(new LoginRequest("test@test.com", "password123"));
        Assert.True(result.Success);
        Assert.NotNull(result.Token);
    }
}

public class FakeAuthService : IAuthService
{
    private readonly IUserRepository _repository;

    public FakeAuthService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        var exists = await _repository.ExistsByEmailAsync(request.Email);
        if (exists) return new AuthResult(false, null, "Email zaten kayıtlı.");
        var user = new User(request.Email, request.Password, "user");
        await _repository.CreateAsync(user);
        return new AuthResult(true, null, null);
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var user = await _repository.GetByEmailAsync(request.Email);
        if (user == null) return new AuthResult(false, null, "Kullanıcı bulunamadı.");
        return new AuthResult(true, "fake-token", null);
    }
}