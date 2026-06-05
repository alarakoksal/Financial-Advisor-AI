using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.Goals.DTOs;
using BitirmeProjem.Domain.Entities;
using MediatR;

namespace BitirmeProjem.Application.Features.Goals.Commands;

public record CreateGoalCommand(Guid UserId, CreateGoalRequest Request) : IRequest<GoalDto>;

public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, GoalDto>
{
    private readonly IApplicationDbContext _context;

    public CreateGoalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GoalDto> Handle(CreateGoalCommand command, CancellationToken cancellationToken)
    {
        var goal = new Goal
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            Title = command.Request.Title,
            TargetAmount = command.Request.TargetAmount,
            CurrentAmount = command.Request.CurrentAmount,
            Deadline = command.Request.Deadline,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Goals.Add(goal);
        await _context.SaveChangesAsync(cancellationToken);

        return new GoalDto(goal.Id, goal.Title, goal.TargetAmount, goal.CurrentAmount, goal.Deadline, goal.CreatedAt, goal.UpdatedAt);
    }
}
