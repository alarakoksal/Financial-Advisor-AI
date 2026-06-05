using BitirmeProjem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeProjem.Infrastructure.Persistence.Configurations;

public class FinancialScoreHistoryConfiguration : IEntityTypeConfiguration<FinancialScoreHistory>
{
    public void Configure(EntityTypeBuilder<FinancialScoreHistory> builder)
    {
        builder.HasKey(h => h.Id);

        builder.HasOne(h => h.User)
               .WithMany()
               .HasForeignKey(h => h.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
