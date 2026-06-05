using BitirmeProjem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeProjem.Infrastructure.Persistence.Configurations;

public class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Title).IsRequired().HasMaxLength(100);
        builder.Property(g => g.TargetAmount).HasPrecision(18, 2);
        builder.Property(g => g.CurrentAmount).HasPrecision(18, 2);
        builder.Property(g => g.Deadline).IsRequired(false);

        builder.HasOne(g => g.User)
               .WithMany()
               .HasForeignKey(g => g.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
