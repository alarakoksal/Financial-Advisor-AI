using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.FinancialProfile.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.FinancialProfile.Commands;

public record CreateFinancialProfileCommand(Guid UserId, CreateFinancialProfileRequest Request) : IRequest<FinancialProfileResponse>;

public class CreateFinancialProfileCommandHandler : IRequestHandler<CreateFinancialProfileCommand, FinancialProfileResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateFinancialProfileCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FinancialProfileResponse> Handle(CreateFinancialProfileCommand command, CancellationToken cancellationToken)
    {
        var exists = await _context.FinancialProfiles.AnyAsync(f => f.UserId == command.UserId, cancellationToken);
        if (exists)
            throw new InvalidOperationException("FINANCIAL_PROFILE_ALREADY_EXISTS");

        var req = command.Request;
        var profile = new Domain.Entities.FinancialProfile
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            MonthlyIncome = req.MonthlyIncome,
            MonthlyExpenses = req.MonthlyExpenses,
            RentExpense = req.RentExpense,
            RentIncome = req.RentIncome,
            SavingsAmount = req.SavingsAmount,
            DebtAmount = req.DebtAmount,
            Currency = req.Currency,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.FinancialProfiles.Add(profile);
        await _context.SaveChangesAsync(cancellationToken);

        return new FinancialProfileResponse(profile.Id, profile.MonthlyIncome, profile.MonthlyExpenses,
            profile.RentExpense, profile.RentIncome, profile.SavingsAmount, profile.DebtAmount,
            profile.Currency, profile.CreatedAt, profile.UpdatedAt);
    }
}
