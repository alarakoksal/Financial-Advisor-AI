namespace BitirmeProjem.Domain.Entities;

public class RiskOption
{
    public Guid Id { get; set; }
    public Guid RiskQuestionId { get; set; }
    public string OptionTextTr { get; set; } = string.Empty;
    public string OptionTextEn { get; set; } = string.Empty;
    public int Score { get; set; }

    public RiskQuestion RiskQuestion { get; set; } = null!;
}
