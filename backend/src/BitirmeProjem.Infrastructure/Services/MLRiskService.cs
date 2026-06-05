using System.Net.Http.Json;
using System.Text.Json.Serialization;
using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace BitirmeProjem.Infrastructure.Services;

public class MLRiskService : IMLRiskService
{
    private readonly HttpClient _httpClient;

    public MLRiskService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["MLService:BaseUrl"]!);
    }

    public async Task<RiskLevel?> PredictAsync(
        List<int> questionScores,
        double savingsRate,
        double debtToIncome,
        double expenseRatio,
        int age,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = new
            {
                question_scores = questionScores,
                savings_rate = savingsRate,
                debt_to_income = debtToIncome,
                expense_ratio = expenseRatio,
                age
            };

            var response = await _httpClient.PostAsJsonAsync("/predict", payload, cancellationToken);
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<MLPredictResponse>(cancellationToken: cancellationToken);
            return result?.RiskLevel switch
            {
                "Conservative" => RiskLevel.Conservative,
                "Balanced"     => RiskLevel.Balanced,
                "Aggressive"   => RiskLevel.Aggressive,
                _              => null
            };
        }
        catch
        {
            return null;
        }
    }

    private record MLPredictResponse([property: JsonPropertyName("risk_level")] string RiskLevel);
}
