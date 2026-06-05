namespace BitirmeProjem.Application.Features.Auth.DTOs;

public record AuthResponse(string Token, string Email, string FirstName, string LastName);
