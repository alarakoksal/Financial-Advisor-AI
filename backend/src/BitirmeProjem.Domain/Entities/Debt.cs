namespace BitirmeProjem.Domain.Entities;

public class Debt
{
    public Guid     Id              { get; set; }
    public Guid     UserId          { get; set; }
    public string   Name            { get; set; } = string.Empty;
    public string   Type            { get; set; } = "personal"; // mortgage, auto, personal, credit_card, other
    public decimal  TotalAmount     { get; set; }
    public decimal  RemainingAmount { get; set; }
    public decimal  InterestRate    { get; set; } // yıllık %
    public decimal  MonthlyPayment  { get; set; }
    public DateOnly StartDate       { get; set; }
    public DateOnly? EndDate        { get; set; }
    public DateTime CreatedAt       { get; set; }
    public DateTime UpdatedAt       { get; set; }

    public User User { get; set; } = null!;
}
