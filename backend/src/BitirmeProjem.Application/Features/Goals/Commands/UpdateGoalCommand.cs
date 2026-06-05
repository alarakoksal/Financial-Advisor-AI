using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.Goals.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Goals.Commands;

public record UpdateGoalCommand(Guid UserId, Guid GoalId, UpdateGoalRequest Request) : IRequest<GoalDto>;

public class UpdateGoalCommandHandler : IRequestHandler<UpdateGoalCommand, GoalDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateGoalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GoalDto> Handle(UpdateGoalCommand command, CancellationToken cancellationToken)
    {
        var goal = await _context.Goals
            .FirstOrDefaultAsync(g => g.Id == command.GoalId && g.UserId == command.UserId, cancellationToken);

        if (goal is null)
            throw new KeyNotFoundException("GOAL_NOT_FOUND");

        goal.Title = command.Request.Title;
        goal.TargetAmount = command.Request.TargetAmount;
        goal.CurrentAmount = command.Request.CurrentAmount;
        goal.Deadline = command.Request.Deadline;
        goal.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new GoalDto(goal.Id, goal.Title, goal.TargetAmount, goal.CurrentAmount, goal.Deadline, goal.CreatedAt, goal.UpdatedAt);
    }
}
