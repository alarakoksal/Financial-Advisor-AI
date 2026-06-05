using BitirmeProjem.Domain.Enums;

namespace BitirmeProjem.Domain.Entities;

public class RiskTestResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int TotalScore { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public RiskLevel? MLRiskLevel { get; set; }
    public DateTime CompletedAt { get; set; }

    public User User { get; set; } = null!;
    public ICollection<RiskTestAnswer> Answers { get; set; } = new List<RiskTestAnswer>();
}
