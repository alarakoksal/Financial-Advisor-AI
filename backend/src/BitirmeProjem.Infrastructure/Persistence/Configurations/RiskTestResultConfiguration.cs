using BitirmeProjem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeProjem.Infrastructure.Persistence.Configurations;

public class RiskTestResultConfiguration : IEntityTypeConfiguration<RiskTestResult>
{
    public void Configure(EntityTypeBuilder<RiskTestResult> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.TotalScore).IsRequired();
        builder.Property(r => r.RiskLevel).IsRequired();
        builder.Property(r => r.MLRiskLevel).IsRequired(false);
        builder.Property(r => r.CompletedAt).IsRequired();

        builder.HasOne(r => r.User)
               .WithMany()
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
