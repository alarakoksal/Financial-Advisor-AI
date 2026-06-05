using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.User.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.User.Commands;

public record UpdateUserProfileCommand(Guid UserId, UpdateUserProfileRequest Request) : IRequest<UpdateUserProfileResponse>;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserProfileCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<UpdateUserProfileResponse> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user is null)
            throw new KeyNotFoundException("USER_NOT_FOUND");

        if (!string.Equals(user.Email, req.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == req.Email && u.Id != command.UserId, cancellationToken);
            if (emailExists)
                throw new InvalidOperationException("EMAIL_ALREADY_EXISTS");
        }

        if (!string.IsNullOrWhiteSpace(req.NewPassword))
        {
            if (string.IsNullOrWhiteSpace(req.CurrentPassword) || !_passwordHasher.Verify(req.CurrentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("INVALID_CURRENT_PASSWORD");

            user.PasswordHash = _passwordHasher.Hash(req.NewPassword);
        }

        user.FirstName = req.FirstName;
        user.LastName = req.LastName;
        user.Email = req.Email;
        user.DateOfBirth = req.DateOfBirth;
        user.PreferredLanguage = req.PreferredLanguage;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateUserProfileResponse(user.FirstName, user.LastName, user.Email, user.DateOfBirth, user.PreferredLanguage);
    }
}
