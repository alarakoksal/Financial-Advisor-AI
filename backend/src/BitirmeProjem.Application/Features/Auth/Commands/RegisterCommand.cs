using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.Auth.DTOs;
using BitirmeProjem.Domain.Entities;
using UserEntity = BitirmeProjem.Domain.Entities.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Auth.Commands;

public record RegisterCommand(RegisterRequest Request) : IRequest<AuthResponse>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public RegisterCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;

        var emailExists = await _context.Users.AnyAsync(u => u.Email == req.Email, cancellationToken);
        if (emailExists)
            throw new InvalidOperationException("EMAIL_ALREADY_EXISTS");

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            FirstName = req.FirstName,
            LastName = req.LastName,
            Email = req.Email,
            PasswordHash = _passwordHasher.Hash(req.Password),
            DateOfBirth = req.DateOfBirth,
            PreferredLanguage = req.PreferredLanguage,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResponse(_tokenService.GenerateToken(user), user.Email, user.FirstName, user.LastName);
    }
}
