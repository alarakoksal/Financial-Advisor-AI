namespace BitirmeProjem.Application.Features.User.DTOs;

public record UpdateUserProfileRequest(
    string FirstName,
    string LastName,
    string Email,
    DateOnly DateOfBirth,
    string PreferredLanguage,
    string? CurrentPassword,
    string? NewPassword
);

public record UpdateUserProfileResponse(
    string FirstName,
    string LastName,
    string Email,
    DateOnly DateOfBirth,
    string PreferredLanguage
);
