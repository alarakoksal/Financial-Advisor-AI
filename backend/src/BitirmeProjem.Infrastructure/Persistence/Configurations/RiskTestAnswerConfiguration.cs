using BitirmeProjem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeProjem.Infrastructure.Persistence.Configurations;

public class RiskTestAnswerConfiguration : IEntityTypeConfiguration<RiskTestAnswer>
{
    public void Configure(EntityTypeBuilder<RiskTestAnswer> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.RiskTestResult)
               .WithMany(r => r.Answers)
               .HasForeignKey(a => a.RiskTestResultId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.RiskQuestion)
               .WithMany()
               .HasForeignKey(a => a.RiskQuestionId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(a => a.RiskOption)
               .WithMany()
               .HasForeignKey(a => a.RiskOptionId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
