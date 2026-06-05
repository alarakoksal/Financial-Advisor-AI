using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.FinancialProfile.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.FinancialProfile.Queries;

public record GetFinancialProfileQuery(Guid UserId) : IRequest<FinancialProfileResponse>;

public class GetFinancialProfileQueryHandler : IRequestHandler<GetFinancialProfileQuery, FinancialProfileResponse>
{
    private readonly IApplicationDbContext _context;

    public GetFinancialProfileQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FinancialProfileResponse> Handle(GetFinancialProfileQuery query, CancellationToken cancellationToken)
    {
        var profile = await _context.FinancialProfiles.FirstOrDefaultAsync(f => f.UserId == query.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("FINANCIAL_PROFILE_NOT_FOUND");

        return new FinancialProfileResponse(profile.Id, profile.MonthlyIncome, profile.MonthlyExpenses,
            profile.RentExpense, profile.RentIncome, profile.SavingsAmount, profile.DebtAmount,
            profile.Currency, profile.CreatedAt, profile.UpdatedAt);
    }
}
