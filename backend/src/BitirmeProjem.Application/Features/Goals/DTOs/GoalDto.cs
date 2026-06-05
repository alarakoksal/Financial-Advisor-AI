namespace BitirmeProjem.Application.Features.Goals.DTOs;

public record GoalDto(
    Guid Id,
    string Title,
    decimal TargetAmount,
    decimal CurrentAmount,
    DateOnly? Deadline,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateGoalRequest(
    string Title,
    decimal TargetAmount,
    decimal CurrentAmount,
    DateOnly? Deadline
);

public record UpdateGoalRequest(
    string Title,
    decimal TargetAmount,
    decimal CurrentAmount,
    DateOnly? Deadline
);
