using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.ScoreHistory;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Analytics;

public record FinancialHealthDto(
    int    Score,
    int    SavingRateScore,
    int    DebtScore,
    int    CashFlowScore,
    int    EmergencyScore
);

public record GetFinancialHealthQuery(Guid UserId) : IRequest<FinancialHealthDto>;

public class GetFinancialHealthQueryHandler : IRequestHandler<GetFinancialHealthQuery, FinancialHealthDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetFinancialHealthQueryHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context  = context;
        _mediator = mediator;
    }

    public async Task<FinancialHealthDto> Handle(GetFinancialHealthQuery request, CancellationToken cancellationToken)
    {
        var fp = await _context.FinancialProfiles
            .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

        if (fp is null)
            return new FinancialHealthDto(0, 0, 0, 0, 0);

        if (fp.MonthlyIncome == 0)
            return new FinancialHealthDto(0, 0, 0, 0, 0);

        var income       = (double)fp.MonthlyIncome;
        var savingRate   = ((double)fp.MonthlyIncome - (double)fp.MonthlyExpenses) / income;
        var debtToIncome = (double)fp.DebtAmount / income;
        var expenseRatio = (double)fp.MonthlyExpenses / income;

        // Tasarruf skoru /30
        int savingScore = savingRate switch
        {
            >= 0.30 => 30,
            >= 0.20 => 22,
            >= 0.10 => 12,
            >= 0    => 5,
            _       => 0
        };

        // Borç skoru /30
        int debtScore = debtToIncome switch
        {
            < 0.15 => 30,
            < 0.30 => 22,
            < 0.45 => 12,
            < 0.60 => 5,
            _      => 0
        };

        // Nakit akış skoru /25
        int cashFlowScore = expenseRatio switch
        {
            < 0.50 => 25,
            < 0.65 => 18,
            < 0.80 => 10,
            < 0.95 => 4,
            _      => 0
        };

        // Acil fon skoru /15 — savings proxy (parmak hesabı)
        var monthlySaving = (double)(fp.MonthlyIncome - fp.MonthlyExpenses);
        int emergencyScore = monthlySaving switch
        {
            >= 0 when savingRate >= 0.25 => 15,
            >= 0 when savingRate >= 0.15 => 10,
            >= 0 when savingRate >= 0.05 => 5,
            _                            => 0
        };

        var totalScore = savingScore + debtScore + cashFlowScore + emergencyScore;

        // Günde bir kez skoru otomatik kaydet
        await _mediator.Send(new RecordScoreCommand(request.UserId, totalScore), cancellationToken);

        return new FinancialHealthDto(
            Score:          totalScore,
            SavingRateScore: savingScore,
            DebtScore:       debtScore,
            CashFlowScore:   cashFlowScore,
            EmergencyScore:  emergencyScore
        );
    }
}
