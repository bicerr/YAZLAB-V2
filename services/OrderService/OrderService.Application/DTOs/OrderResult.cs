namespace OrderService.Application.DTOs;

public record OrderResult(bool Success, string? ErrorMessage);