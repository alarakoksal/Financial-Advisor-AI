using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Auth.Queries;

public record LoginQuery(LoginRequest Request) : IRequest<AuthResponse>;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginQueryHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var req = query.Request;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == req.Email, cancellationToken)
            ?? throw new UnauthorizedAccessException("Email veya şifre hatalı.");

        if (!_passwordHasher.Verify(req.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email veya şifre hatalı.");

        return new AuthResponse(_tokenService.GenerateToken(user), user.Email, user.FirstName, user.LastName);
    }
}
