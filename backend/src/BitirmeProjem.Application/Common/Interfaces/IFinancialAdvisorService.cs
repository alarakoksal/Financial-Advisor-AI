namespace BitirmeProjem.Application.Common.Interfaces;

public interface IFinancialAdvisorService
{
    Task<string> GetAdviceAsync(FinancialAdvisorContext context, CancellationToken cancellationToken = default);
}

public record FinancialAdvisorContext(
    string PreferredLanguage,
    string Currency,
    int Age,
    string RiskLevel,
    string? MLRiskLevel,
    int TotalRiskScore,
    decimal MonthlyIncome,
    decimal MonthlyExpenses,
    decimal SavingsAmount,
    double SavingsRate,
    decimal DebtAmount,
    double DebtToIncomeRatio,
    decimal RentExpense,
    decimal RentIncome,
    List<GoalContext> Goals
);

public record GoalContext(
    string Title,
    decimal TargetAmount,
    decimal CurrentAmount,
    DateOnly? Deadline
);
