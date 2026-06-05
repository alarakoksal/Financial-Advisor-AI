using BitirmeProjem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitirmeProjem.Infrastructure.Persistence.Configurations;

public class DebtConfiguration : IEntityTypeConfiguration<Debt>
{
    public void Configure(EntityTypeBuilder<Debt> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(200);
        builder.Property(d => d.Type).IsRequired().HasMaxLength(50);
        builder.Property(d => d.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(d => d.RemainingAmount).HasColumnType("decimal(18,2)");
        builder.Property(d => d.InterestRate).HasColumnType("decimal(5,2)");
        builder.Property(d => d.MonthlyPayment).HasColumnType("decimal(18,2)");

        builder.HasOne(d => d.User)
               .WithMany()
               .HasForeignKey(d => d.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
