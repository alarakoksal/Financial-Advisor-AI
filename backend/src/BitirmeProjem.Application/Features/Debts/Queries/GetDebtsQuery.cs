using BitirmeProjem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.Debts.Queries;

public record DebtDto(
    Guid    Id,
    string  Name,
    string  Type,
    decimal TotalAmount,
    decimal RemainingAmount,
    decimal InterestRate,
    decimal MonthlyPayment,
    DateOnly StartDate,
    DateOnly? EndDate,
    int     RemainingMonths,
    decimal TotalInterestCost,
    double  PaidPercent
);

public record GetDebtsQuery(Guid UserId) : IRequest<List<DebtDto>>;

public class GetDebtsQueryHandler : IRequestHandler<GetDebtsQuery, List<DebtDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDebtsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<DebtDto>> Handle(GetDebtsQuery request, CancellationToken cancellationToken)
    {
        var debts = await _context.Debts
            .Where(d => d.UserId == request.UserId)
            .OrderBy(d => d.EndDate)
            .ToListAsync(cancellationToken);

        return debts.Select(d =>
        {
            var remainingMonths = d.MonthlyPayment > 0
                ? (int)Math.Ceiling((double)d.RemainingAmount / (double)d.MonthlyPayment)
                : 0;

            var monthlyRate = d.InterestRate / 100 / 12;
            var totalInterest = monthlyRate > 0 && remainingMonths > 0
                ? (double)d.MonthlyPayment * remainingMonths - (double)d.RemainingAmount
                : 0;

            var paidPercent = d.TotalAmount > 0
                ? (double)(d.TotalAmount - d.RemainingAmount) / (double)d.TotalAmount * 100
                : 0;

            return new DebtDto(
                d.Id, d.Name, d.Type,
                d.TotalAmount, d.RemainingAmount, d.InterestRate, d.MonthlyPayment,
                d.StartDate, d.EndDate,
                remainingMonths, (decimal)Math.Max(0, totalInterest),
                Math.Round(paidPercent, 1));
        }).ToList();
    }
}
