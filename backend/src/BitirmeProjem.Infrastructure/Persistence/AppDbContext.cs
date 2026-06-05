using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<FinancialProfile> FinancialProfiles => Set<FinancialProfile>();
    public DbSet<RiskQuestion> RiskQuestions => Set<RiskQuestion>();
    public DbSet<RiskOption> RiskOptions => Set<RiskOption>();
    public DbSet<RiskTestResult> RiskTestResults => Set<RiskTestResult>();
    public DbSet<RiskTestAnswer> RiskTestAnswers => Set<RiskTestAnswer>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<AIRecommendation> AIRecommendations => Set<AIRecommendation>();
    public DbSet<Debt> Debts => Set<Debt>();
    public DbSet<FinancialScoreHistory> FinancialScoreHistories => Set<FinancialScoreHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
