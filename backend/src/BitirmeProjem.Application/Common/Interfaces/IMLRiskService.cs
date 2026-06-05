using BitirmeProjem.Domain.Enums;

namespace BitirmeProjem.Application.Common.Interfaces;

public interface IMLRiskService
{
    Task<RiskLevel?> PredictAsync(
        List<int> questionScores,
        double savingsRate,
        double debtToIncome,
        double expenseRatio,
        int age,
        CancellationToken cancellationToken = default);
}
