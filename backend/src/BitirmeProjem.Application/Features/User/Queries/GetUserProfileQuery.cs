using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.User.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.User.Queries;

public record GetUserProfileQuery(Guid UserId) : IRequest<UpdateUserProfileResponse>;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UpdateUserProfileResponse>
{
    private readonly IApplicationDbContext _context;

    public GetUserProfileQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateUserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user is null)
            throw new KeyNotFoundException("USER_NOT_FOUND");

        return new UpdateUserProfileResponse(user.FirstName, user.LastName, user.Email, user.DateOfBirth, user.PreferredLanguage);
    }
}
