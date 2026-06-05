namespace BitirmeProjem.Application.Features.FinancialProfile.DTOs;

public record CreateFinancialProfileRequest(
    decimal MonthlyIncome,
    decimal MonthlyExpenses,
    decimal RentExpense,
    decimal RentIncome,
    decimal SavingsAmount,
    decimal DebtAmount,
    string Currency
);
