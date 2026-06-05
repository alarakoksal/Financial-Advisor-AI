namespace BitirmeProjem.Application.Features.FinancialProfile.DTOs;

public record FinancialProfileResponse(
    Guid Id,
    decimal MonthlyIncome,
    decimal MonthlyExpenses,
    decimal RentExpense,
    decimal RentIncome,
    decimal SavingsAmount,
    decimal DebtAmount,
    string Currency,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
