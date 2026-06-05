using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Dashboard;

public record NearestGoalDto(
    string   Title,
    decimal  TargetAmount,
    decimal  CurrentAmount,
    string   Deadline
);

public record DashboardSummaryDto(
    string         FullName,
    string?        RiskLevel,
    decimal        MonthlyIncome,
    decimal        MonthlyExpenses,
    double         SavingRate,
    double         DebtToIncomeRatio,
    double         ExpenseRatio,
    int            GoalCount,
    NearestGoalDto? NearestGoal,
    int            NotificationCount,
    string?        LatestAdvice
);

public record GetDashboardSummaryQuery(Guid UserId) : IRequest<DashboardSummaryDto>;

public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetDashboardSummaryQueryHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.FinancialProfile)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("USER_NOT_FOUND");

        if (user.FinancialProfile is null)
            return new DashboardSummaryDto(
                FullName:          $"{user.FirstName} {user.LastName}",
                RiskLevel:         null,
                MonthlyIncome:     0,
                MonthlyExpenses:   0,
                SavingRate:        0,
                DebtToIncomeRatio: 0,
                ExpenseRatio:      0,
                GoalCount:         0,
                NearestGoal:       null,
                NotificationCount: 0,
                LatestAdvice:      null
            );

        var fp = user.FinancialProfile;

        double savingRate   = 0;
        double debtToIncome = 0;
        double expenseRatio = 0;

        if (fp.MonthlyIncome > 0)
        {
            var income   = (double)fp.MonthlyIncome;
            savingRate   = Math.Round(((double)fp.MonthlyIncome - (double)fp.MonthlyExpenses) / income, 4);
            debtToIncome = Math.Round((double)fp.DebtAmount / income, 4);
            expenseRatio = Math.Round((double)fp.MonthlyExpenses / income, 4);
        }

        var latestResult = await _context.RiskTestResults
            .Where(r => r.UserId == request.UserId)
            .OrderByDescending(r => r.CompletedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var riskLevel = latestResult is null
            ? null
            : (latestResult.MLRiskLevel ?? latestResult.RiskLevel).ToString();

        var goals = await _context.Goals
            .Where(g => g.UserId == request.UserId)
            .OrderBy(g => g.Deadline)
            .ToListAsync(cancellationToken);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var nearest = goals
            .Where(g => g.Deadline.HasValue && g.Deadline.Value >= today)
            .OrderBy(g => g.Deadline)
            .FirstOrDefault()
            ?? goals.FirstOrDefault();

        NearestGoalDto? nearestGoalDto = nearest is null ? null : new NearestGoalDto(
            Title:         nearest.Title,
            TargetAmount:  nearest.TargetAmount,
            CurrentAmount: nearest.CurrentAmount,
            Deadline:      nearest.Deadline?.ToString("yyyy-MM-dd") ?? today.ToString("yyyy-MM-dd")
        );

        List<Notifications.NotificationDto> notifications = [];
        try
        {
            notifications = await _mediator.Send(new GetNotificationsQuery(request.UserId), cancellationToken);
        }
        catch { /* notifications optional */ }

        var latestAdvice = await _context.AIRecommendations
            .Where(r => r.UserId == request.UserId)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => r.Content)
            .FirstOrDefaultAsync(cancellationToken);

        var adviceSummary = latestAdvice is null
            ? null
            : (latestAdvice.Length > 150 ? latestAdvice[..150].TrimEnd() + "..." : latestAdvice);

        return new DashboardSummaryDto(
            FullName:          $"{user.FirstName} {user.LastName}",
            RiskLevel:         riskLevel,
            MonthlyIncome:     fp.MonthlyIncome,
            MonthlyExpenses:   fp.MonthlyExpenses,
            SavingRate:        savingRate,
            DebtToIncomeRatio: debtToIncome,
            ExpenseRatio:      expenseRatio,
            GoalCount:         goals.Count,
            NearestGoal:       nearestGoalDto,
            NotificationCount: notifications.Count,
            LatestAdvice:      adviceSummary
        );
    }
}
