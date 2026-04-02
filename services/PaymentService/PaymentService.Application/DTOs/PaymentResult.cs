namespace PaymentService.Application.DTOs;

public record PaymentResult(bool Success, string? ErrorMessage);