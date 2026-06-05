namespace BitirmeProjem.Domain.Entities;

public class RiskQuestion
{
    public Guid Id { get; set; }
    public int OrderIndex { get; set; }
    public string QuestionTextTr { get; set; } = string.Empty;
    public string QuestionTextEn { get; set; } = string.Empty;

    public ICollection<RiskOption> Options { get; set; } = new List<RiskOption>();
}
