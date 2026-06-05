using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.Debts.Queries;
using BitirmeProjem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Debts.Commands;

// ── Shared request ──────────────────────────────────────────────
public record DebtRequest(
    string   Name,
    string   Type,
    decimal  TotalAmount,
    decimal  RemainingAmount,
    decimal  InterestRate,
    decimal  MonthlyPayment,
    DateOnly StartDate,
    DateOnly? EndDate
);

// ── Create ──────────────────────────────────────────────────────
public record CreateDebtCommand(Guid UserId, DebtRequest Request) : IRequest<DebtDto>;

public class CreateDebtCommandHandler : IRequestHandler<CreateDebtCommand, DebtDto>
{
    private readonly IApplicationDbContext _context;

    public CreateDebtCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<DebtDto> Handle(CreateDebtCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var debt = new Debt
        {
            Id              = Guid.NewGuid(),
            UserId          = command.UserId,
            Name            = req.Name,
            Type            = req.Type,
            TotalAmount     = req.TotalAmount,
            RemainingAmount = req.RemainingAmount,
            InterestRate    = req.InterestRate,
            MonthlyPayment  = req.MonthlyPayment,
            StartDate       = req.StartDate,
            EndDate         = req.EndDate,
            CreatedAt       = DateTime.UtcNow,
            UpdatedAt       = DateTime.UtcNow,
        };

        _context.Debts.Add(debt);
        await _context.SaveChangesAsync(cancellationToken);

        return ToDto(debt);
    }

    private static DebtDto ToDto(Debt d)
    {
        var remaining = d.MonthlyPayment > 0 ? (int)Math.Ceiling((double)d.RemainingAmount / (double)d.MonthlyPayment) : 0;
        var paid = d.TotalAmount > 0 ? (double)(d.TotalAmount - d.RemainingAmount) / (double)d.TotalAmount * 100 : 0;
        return new DebtDto(d.Id, d.Name, d.Type, d.TotalAmount, d.RemainingAmount, d.InterestRate, d.MonthlyPayment, d.StartDate, d.EndDate, remaining, 0, Math.Round(paid, 1));
    }
}

// ── Update ──────────────────────────────────────────────────────
public record UpdateDebtCommand(Guid UserId, Guid DebtId, DebtRequest Request) : IRequest<DebtDto>;

public class UpdateDebtCommandHandler : IRequestHandler<UpdateDebtCommand, DebtDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateDebtCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<DebtDto> Handle(UpdateDebtCommand command, CancellationToken cancellationToken)
    {
        var debt = await _context.Debts
            .FirstOrDefaultAsync(d => d.Id == command.DebtId && d.UserId == command.UserId, cancellationToken);

        if (debt is null) throw new KeyNotFoundException("DEBT_NOT_FOUND");

        var req = command.Request;
        debt.Name            = req.Name;
        debt.Type            = req.Type;
        debt.TotalAmount     = req.TotalAmount;
        debt.RemainingAmount = req.RemainingAmount;
        debt.InterestRate    = req.InterestRate;
        debt.MonthlyPayment  = req.MonthlyPayment;
        debt.StartDate       = req.StartDate;
        debt.EndDate         = req.EndDate;
        debt.UpdatedAt       = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var remaining = debt.MonthlyPayment > 0 ? (int)Math.Ceiling((double)debt.RemainingAmount / (double)debt.MonthlyPayment) : 0;
        var paid = debt.TotalAmount > 0 ? (double)(debt.TotalAmount - debt.RemainingAmount) / (double)debt.TotalAmount * 100 : 0;
        return new DebtDto(debt.Id, debt.Name, debt.Type, debt.TotalAmount, debt.RemainingAmount, debt.InterestRate, debt.MonthlyPayment, debt.StartDate, debt.EndDate, remaining, 0, Math.Round(paid, 1));
    }
}

// ── Delete ──────────────────────────────────────────────────────
public record DeleteDebtCommand(Guid UserId, Guid DebtId) : IRequest;

public class DeleteDebtCommandHandler : IRequestHandler<DeleteDebtCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDebtCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteDebtCommand command, CancellationToken cancellationToken)
    {
        var debt = await _context.Debts
            .FirstOrDefaultAsync(d => d.Id == command.DebtId && d.UserId == command.UserId, cancellationToken);

        if (debt is null) throw new KeyNotFoundException("DEBT_NOT_FOUND");

        _context.Debts.Remove(debt);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
