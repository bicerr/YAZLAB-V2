namespace AuthService.Application.DTOs;

public record AuthResult(bool Success, string? Token, string? ErrorMessage);