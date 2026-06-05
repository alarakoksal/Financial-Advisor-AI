using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.RiskTest.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.RiskTest.Queries;

public record GetLatestRiskTestResultQuery(Guid UserId) : IRequest<RiskTestResultResponse>;

public class GetLatestRiskTestResultQueryHandler : IRequestHandler<GetLatestRiskTestResultQuery, RiskTestResultResponse>
{
    private readonly IApplicationDbContext _context;

    public GetLatestRiskTestResultQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RiskTestResultResponse> Handle(GetLatestRiskTestResultQuery query, CancellationToken cancellationToken)
    {
        var result = await _context.RiskTestResults
            .Where(r => r.UserId == query.UserId)
            .OrderByDescending(r => r.CompletedAt)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KeyNotFoundException("RISK_TEST_RESULT_NOT_FOUND");

        return new RiskTestResultResponse(result.Id, result.TotalScore, result.RiskLevel, result.MLRiskLevel, result.CompletedAt);
    }
}
