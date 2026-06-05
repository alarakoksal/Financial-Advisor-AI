namespace BitirmeProjem.Application.Features.RiskTest.DTOs;

public record RiskQuestionDto(Guid Id, int OrderIndex, string QuestionTextTr, string QuestionTextEn, List<RiskOptionDto> Options);
