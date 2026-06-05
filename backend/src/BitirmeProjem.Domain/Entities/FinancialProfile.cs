namespace BitirmeProjem.Domain.Entities;

public class FinancialProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public decimal MonthlyIncome { get; set; }
    public decimal MonthlyExpenses { get; set; }
    public decimal RentExpense { get; set; }
    public decimal RentIncome { get; set; }
    public decimal SavingsAmount { get; set; }
    public decimal DebtAmount { get; set; }
    public string Currency { get; set; } = "TRY";

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
}
