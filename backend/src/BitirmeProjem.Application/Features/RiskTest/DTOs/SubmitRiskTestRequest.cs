namespace BitirmeProjem.Application.Features.RiskTest.DTOs;

public record SubmitRiskTestRequest(List<RiskTestAnswerItem> Answers);

public record RiskTestAnswerItem(Guid QuestionId, Guid OptionId);
