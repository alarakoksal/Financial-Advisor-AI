using BitirmeProjem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Advice;

public record GetFinancialAdviceQuery(Guid UserId) : IRequest<string>;

public class GetFinancialAdviceQueryHandler : IRequestHandler<GetFinancialAdviceQuery, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IFinancialAdvisorService _advisorService;

    public GetFinancialAdviceQueryHandler(IApplicationDbContext context, IFinancialAdvisorService advisorService)
    {
        _context = context;
        _advisorService = advisorService;
    }

    public async Task<string> Handle(GetFinancialAdviceQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.FinancialProfile)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user?.FinancialProfile is null)
            throw new KeyNotFoundException("FINANCIAL_PROFILE_NOT_FOUND");

        var latestResult = await _context.RiskTestResults
            .Where(r => r.UserId == request.UserId)
            .OrderByDescending(r => r.CompletedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var goals = await _context.Goals
            .Where(g => g.UserId == request.UserId)
            .OrderBy(g => g.CreatedAt)
            .ToListAsync(cancellationToken);

        var fp = user.FinancialProfile;
        var age = DateTime.UtcNow.Year - user.DateOfBirth.Year;
        var savingsRate = fp.MonthlyIncome > 0 ? (double)(fp.MonthlyIncome - fp.MonthlyExpenses) / (double)fp.MonthlyIncome : 0;
        var debtToIncome = fp.MonthlyIncome > 0 ? (double)(fp.DebtAmount / fp.MonthlyIncome) : 0;

        var ctx = new FinancialAdvisorContext(
            PreferredLanguage:  user.PreferredLanguage,
            Currency:           fp.Currency,
            Age:                age,
            RiskLevel:          latestResult?.RiskLevel.ToString() ?? "Unknown",
            MLRiskLevel:        latestResult?.MLRiskLevel?.ToString(),
            TotalRiskScore:     latestResult?.TotalScore ?? 0,
            MonthlyIncome:      fp.MonthlyIncome,
            MonthlyExpenses:    fp.MonthlyExpenses,
            SavingsAmount:      fp.SavingsAmount,
            SavingsRate:        savingsRate,
            DebtAmount:         fp.DebtAmount,
            DebtToIncomeRatio:  debtToIncome,
            RentExpense:        fp.RentExpense,
            RentIncome:         fp.RentIncome,
            Goals:              goals.Select(g => new GoalContext(g.Title, g.TargetAmount, g.CurrentAmount, g.Deadline)).ToList()
        );

        var content = await _advisorService.GetAdviceAsync(ctx, cancellationToken);

        _context.AIRecommendations.Add(new Domain.Entities.AIRecommendation
        {
            Id        = Guid.NewGuid(),
            UserId    = request.UserId,
            Content   = content,
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync(cancellationToken);

        return content;
    }
}
