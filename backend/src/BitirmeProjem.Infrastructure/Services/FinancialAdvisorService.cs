using BitirmeProjem.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

namespace BitirmeProjem.Infrastructure.Services;

public class FinancialAdvisorService : IFinancialAdvisorService
{
    private readonly ChatClient _chatClient;

    private const string SystemPrompt =
        """
        You are an AI-powered personal finance assistant. Your role is to analyze the user's financial data, interpret their financial situation, and provide detailed, personalized guidance.

        Your purpose:
        - Increase the user's financial awareness
        - Objectively interpret their financial situation
        - Provide concrete, in-depth suggestions on budget management, savings, debt reduction, and realistic goal planning

        Rules you must strictly follow:
        - Never give direct investment advice. Do not say things like "buy this stock", "invest in this fund", or "guaranteed returns".
        - Never guarantee any financial returns.
        - Do not fabricate or assume information that has not been provided to you.
        - Always consider the user's risk profile (RiskLevel and MLRiskLevel). If their financial situation is risky or they have a conservative profile, avoid aggressive suggestions.
        - Clearly explain financial risks when relevant.
        - Always prioritize financial safety.

        Response style:
        - Detailed, clear, and professional.
        - Cover all relevant aspects of the user's financial situation — do not give a superficial summary.
        - Make sure every suggestion is specific and actionable; avoid vague or generic statements.
        - Structure the response with clear sections and use bullet points within each section.
        - Always respond in the language specified at the end of the user message.
        """;

    public FinancialAdvisorService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"]!;
        var client = new OpenAIClient(apiKey);
        _chatClient = client.GetChatClient("gpt-3.5-turbo");
    }

    public async Task<string> GetAdviceAsync(FinancialAdvisorContext ctx, CancellationToken cancellationToken = default)
    {
        var isTurkish = ctx.PreferredLanguage != "en";
        var userPrompt = BuildUserPrompt(ctx, isTurkish);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(SystemPrompt),
            new UserChatMessage(userPrompt)
        };

        var response = await _chatClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);
        return response.Value.Content[0].Text;
    }

    private static string BuildUserPrompt(FinancialAdvisorContext ctx, bool isTurkish)
    {
        var mlRisk = ctx.MLRiskLevel ?? (isTurkish ? "Henüz hesaplanmadı" : "Not calculated yet");
        var netCashFlow = ctx.MonthlyIncome - ctx.MonthlyExpenses;
        var healthScore = CalculateHealthScore(ctx);
        var healthLabel = healthScore switch
        {
            >= 70 => isTurkish ? "İyi" : "Good",
            >= 40 => isTurkish ? "Orta" : "Moderate",
            _     => isTurkish ? "Riskli" : "At Risk"
        };

        var goalsText = ctx.Goals.Count == 0
            ? (isTurkish ? "Tanımlanmış hedef yok." : "No goals defined.")
            : string.Join("\n", ctx.Goals.Select(g =>
            {
                var remaining = g.TargetAmount - g.CurrentAmount;
                var daysLeft = g.Deadline.HasValue
                    ? (g.Deadline.Value.ToDateTime(TimeOnly.MinValue) - DateTime.UtcNow).Days
                    : (int?)null;
                var deadlineText = g.Deadline.HasValue
                    ? (isTurkish ? $"{g.Deadline.Value:yyyy-MM-dd} ({daysLeft} gün kaldı)" : $"{g.Deadline.Value:yyyy-MM-dd} ({daysLeft} days left)")
                    : (isTurkish ? "belirtilmemiş" : "not specified");

                return isTurkish
                    ? $"  - {g.Title}: Hedef {g.TargetAmount:N0} {ctx.Currency}, Mevcut {g.CurrentAmount:N0} {ctx.Currency}, Kalan {remaining:N0} {ctx.Currency}, Son tarih: {deadlineText}"
                    : $"  - {g.Title}: Target {g.TargetAmount:N0} {ctx.Currency}, Current {g.CurrentAmount:N0} {ctx.Currency}, Remaining {remaining:N0} {ctx.Currency}, Deadline: {deadlineText}";
            }));

        return $"""
               Context: This user wants to understand and improve their financial situation. Provide a thorough analysis and actionable guidance based on the data below.

               Financial Health Score: {healthScore}/100 ({healthLabel})

               Risk Profile:
               - Rule-based risk level: {ctx.RiskLevel}
               - ML-based risk level: {mlRisk}
               - Total risk score: {ctx.TotalRiskScore} / 40

               Demographics:
               - Age: {ctx.Age}

               Financial Status ({ctx.Currency}):
               - Monthly income: {ctx.MonthlyIncome:N0}
               - Monthly expenses: {ctx.MonthlyExpenses:N0}
               - Net monthly cash flow: {netCashFlow:N0} ({(netCashFlow >= 0 ? "surplus" : "deficit")})
               - Savings amount: {ctx.SavingsAmount:N0}
               - Savings rate: {ctx.SavingsRate * 100:F1}%
               - Total debt: {ctx.DebtAmount:N0}
               - Debt-to-income ratio: {ctx.DebtToIncomeRatio * 100:F1}%
               - Rent expense: {ctx.RentExpense:N0}
               - Rental income: {ctx.RentIncome:N0}

               Financial Goals:
               {goalsText}

               Analyze the user's financial situation thoroughly and provide detailed, personalized advice covering all relevant areas (budget, savings, debt, goals, risks).

               IMPORTANT: Respond in {(isTurkish ? "Turkish" : "English")}.
               """;
    }

    private static int CalculateHealthScore(FinancialAdvisorContext ctx)
    {
        var score = 0;

        // Tasarruf oranı (max 30 puan)
        score += ctx.SavingsRate switch
        {
            >= 0.30 => 30,
            >= 0.20 => 22,
            >= 0.10 => 14,
            >= 0.05 => 7,
            _       => 0
        };

        // Borç/Gelir oranı (max 30 puan)
        score += ctx.DebtToIncomeRatio switch
        {
            <= 0.10 => 30,
            <= 0.20 => 22,
            <= 0.35 => 14,
            <= 0.50 => 7,
            _       => 0
        };

        // Net nakit akışı (max 25 puan)
        var netFlow = ctx.MonthlyIncome - ctx.MonthlyExpenses;
        score += netFlow switch
        {
            > 0 when (double)netFlow / (double)ctx.MonthlyIncome >= 0.30 => 25,
            > 0 when (double)netFlow / (double)ctx.MonthlyIncome >= 0.15 => 18,
            > 0 => 10,
            _   => 0
        };

        // Acil durum fonu (3 aylık gider birikimi) (max 15 puan)
        var monthsCovered = ctx.MonthlyExpenses > 0 ? (double)(ctx.SavingsAmount / ctx.MonthlyExpenses) : 0;
        score += monthsCovered switch
        {
            >= 6 => 15,
            >= 3 => 10,
            >= 1 => 5,
            _    => 0
        };

        return Math.Min(score, 100);
    }
}
