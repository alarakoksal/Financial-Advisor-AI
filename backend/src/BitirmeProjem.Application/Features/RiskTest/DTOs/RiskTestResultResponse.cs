using BitirmeProjem.Domain.Enums;

namespace BitirmeProjem.Application.Features.RiskTest.DTOs;

public record RiskTestResultResponse(
    Guid Id,
    int TotalScore,
    RiskLevel RiskLevel,
    RiskLevel? MLRiskLevel,
    DateTime CompletedAt
);
