namespace BitirmeProjem.Domain.Entities;

public class RiskTestAnswer
{
    public Guid Id { get; set; }
    public Guid RiskTestResultId { get; set; }
    public Guid RiskQuestionId { get; set; }
    public Guid RiskOptionId { get; set; }

    public RiskTestResult RiskTestResult { get; set; } = null!;
    public RiskQuestion RiskQuestion { get; set; } = null!;
    public RiskOption RiskOption { get; set; } = null!;
}
