namespace BitirmeProjem.Application.Features.FinancialProfile.DTOs;

public record UpdateFinancialProfileRequest(
    decimal MonthlyIncome,
    decimal MonthlyExpenses,
    decimal RentExpense,
    decimal RentIncome,
    decimal SavingsAmount,
    decimal DebtAmount,
    string Currency
);
