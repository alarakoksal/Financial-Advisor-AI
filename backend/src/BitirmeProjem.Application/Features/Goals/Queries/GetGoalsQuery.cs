using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.Goals.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Goals.Queries;

public record GetGoalsQuery(Guid UserId) : IRequest<List<GoalDto>>;

public class GetGoalsQueryHandler : IRequestHandler<GetGoalsQuery, List<GoalDto>>
{
    private readonly IApplicationDbContext _context;

    public GetGoalsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<GoalDto>> Handle(GetGoalsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Goals
            .Where(g => g.UserId == request.UserId)
            .OrderBy(g => g.CreatedAt)
            .Select(g => new GoalDto(g.Id, g.Title, g.TargetAmount, g.CurrentAmount, g.Deadline, g.CreatedAt, g.UpdatedAt))
            .ToListAsync(cancellationToken);
    }
}
