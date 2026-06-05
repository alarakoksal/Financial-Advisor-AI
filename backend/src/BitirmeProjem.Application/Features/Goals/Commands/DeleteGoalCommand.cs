using BitirmeProjem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Goals.Commands;

public record DeleteGoalCommand(Guid UserId, Guid GoalId) : IRequest;

public class DeleteGoalCommandHandler : IRequestHandler<DeleteGoalCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteGoalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteGoalCommand command, CancellationToken cancellationToken)
    {
        var goal = await _context.Goals
            .FirstOrDefaultAsync(g => g.Id == command.GoalId && g.UserId == command.UserId, cancellationToken);

        if (goal is null)
            throw new KeyNotFoundException("GOAL_NOT_FOUND");

        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
