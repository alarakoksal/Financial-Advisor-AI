using BitirmeProjem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<FinancialProfile> FinancialProfiles { get; }
    DbSet<RiskQuestion> RiskQuestions { get; }
    DbSet<RiskOption> RiskOptions { get; }
    DbSet<RiskTestResult> RiskTestResults { get; }
    DbSet<RiskTestAnswer> RiskTestAnswers { get; }
    DbSet<Goal> Goals { get; }
    DbSet<AIRecommendation> AIRecommendations { get; }
    DbSet<Debt> Debts { get; }
    DbSet<FinancialScoreHistory> FinancialScoreHistories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
