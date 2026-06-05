namespace BitirmeProjem.Domain.Entities;

public class FinancialScoreHistory
{
    public Guid     Id         { get; set; }
    public Guid     UserId     { get; set; }
    public int      Score      { get; set; }
    public DateTime RecordedAt { get; set; }

    public User User { get; set; } = null!;
}
