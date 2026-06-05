using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.ScoreHistory;

public record ScoreHistoryDto(int Score, DateTime RecordedAt);

// ── Get history ─────────────────────────────────────────────────
public record GetScoreHistoryQuery(Guid UserId) : IRequest<List<ScoreHistoryDto>>;

public class GetScoreHistoryQueryHandler : IRequestHandler<GetScoreHistoryQuery, List<ScoreHistoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetScoreHistoryQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<ScoreHistoryDto>> Handle(GetScoreHistoryQuery request, CancellationToken cancellationToken)
    {
        return await _context.FinancialScoreHistories
            .Where(h => h.UserId == request.UserId)
            .OrderBy(h => h.RecordedAt)
            .Select(h => new ScoreHistoryDto(h.Score, h.RecordedAt))
            .ToListAsync(cancellationToken);
    }
}

// ── Record score (günde 1 kez) ──────────────────────────────────
public record RecordScoreCommand(Guid UserId, int Score) : IRequest;

public class RecordScoreCommandHandler : IRequestHandler<RecordScoreCommand>
{
    private readonly IApplicationDbContext _context;

    public RecordScoreCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(RecordScoreCommand request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var alreadyRecorded = await _context.FinancialScoreHistories
            .AnyAsync(h => h.UserId == request.UserId && h.RecordedAt.Date == today, cancellationToken);

        if (alreadyRecorded) return;

        _context.FinancialScoreHistories.Add(new FinancialScoreHistory
        {
            Id         = Guid.NewGuid(),
            UserId     = request.UserId,
            Score      = request.Score,
            RecordedAt = DateTime.UtcNow,
        });

        await _context.SaveChangesAsync(cancellationToken);
    }
}
