using BitirmeProjem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeProjem.Infrastructure.Persistence.Configurations;

public class FinancialProfileConfiguration : IEntityTypeConfiguration<FinancialProfile>
{
    public void Configure(EntityTypeBuilder<FinancialProfile> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.MonthlyIncome).HasPrecision(18, 2);
        builder.Property(f => f.MonthlyExpenses).HasPrecision(18, 2);
        builder.Property(f => f.RentExpense).HasPrecision(18, 2);
        builder.Property(f => f.RentIncome).HasPrecision(18, 2);
        builder.Property(f => f.SavingsAmount).HasPrecision(18, 2);
        builder.Property(f => f.DebtAmount).HasPrecision(18, 2);
        builder.Property(f => f.Currency).IsRequired().HasMaxLength(10);

        builder.HasIndex(f => f.UserId).IsUnique();

        builder.HasOne(f => f.User)
               .WithOne(u => u.FinancialProfile)
               .HasForeignKey<FinancialProfile>(f => f.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
