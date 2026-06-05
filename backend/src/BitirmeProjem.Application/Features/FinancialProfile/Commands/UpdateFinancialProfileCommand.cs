using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.FinancialProfile.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.FinancialProfile.Commands;

public record UpdateFinancialProfileCommand(Guid UserId, UpdateFinancialProfileRequest Request) : IRequest<FinancialProfileResponse>;

public class UpdateFinancialProfileCommandHandler : IRequestHandler<UpdateFinancialProfileCommand, FinancialProfileResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateFinancialProfileCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FinancialProfileResponse> Handle(UpdateFinancialProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await _context.FinancialProfiles.FirstOrDefaultAsync(f => f.UserId == command.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("FINANCIAL_PROFILE_NOT_FOUND");

        var req = command.Request;
        profile.MonthlyIncome = req.MonthlyIncome;
        profile.MonthlyExpenses = req.MonthlyExpenses;
        profile.RentExpense = req.RentExpense;
        profile.RentIncome = req.RentIncome;
        profile.SavingsAmount = req.SavingsAmount;
        profile.DebtAmount = req.DebtAmount;
        profile.Currency = req.Currency;
        profile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new FinancialProfileResponse(profile.Id, profile.MonthlyIncome, profile.MonthlyExpenses,
            profile.RentExpense, profile.RentIncome, profile.SavingsAmount, profile.DebtAmount,
            profile.Currency, profile.CreatedAt, profile.UpdatedAt);
    }
}
