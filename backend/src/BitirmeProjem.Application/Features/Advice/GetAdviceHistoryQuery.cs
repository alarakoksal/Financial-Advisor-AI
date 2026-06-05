using BitirmeProjem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Advice;

public record AdviceHistoryItem(Guid Id, string Content, DateTime CreatedAt);

public record GetAdviceHistoryQuery(Guid UserId) : IRequest<List<AdviceHistoryItem>>;

public class GetAdviceHistoryQueryHandler : IRequestHandler<GetAdviceHistoryQuery, List<AdviceHistoryItem>>
{
    private readonly IApplicationDbContext _context;

    public GetAdviceHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AdviceHistoryItem>> Handle(GetAdviceHistoryQuery request, CancellationToken cancellationToken)
    {
        return await _context.AIRecommendations
            .Where(r => r.UserId == request.UserId)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new AdviceHistoryItem(r.Id, r.Content, r.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
