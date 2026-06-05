using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.RiskTest.DTOs;
using BitirmeProjem.Domain.Entities;
using BitirmeProjem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.RiskTest.Commands;

public record SubmitRiskTestCommand(Guid UserId, SubmitRiskTestRequest Request) : IRequest<RiskTestResultResponse>;

public class SubmitRiskTestCommandHandler : IRequestHandler<SubmitRiskTestCommand, RiskTestResultResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMLRiskService _mlRiskService;

    public SubmitRiskTestCommandHandler(IApplicationDbContext context, IMLRiskService mlRiskService)
    {
        _context = context;
        _mlRiskService = mlRiskService;
    }

    public async Task<RiskTestResultResponse> Handle(SubmitRiskTestCommand command, CancellationToken cancellationToken)
    {
        var answers = command.Request.Answers;

        if (answers.Count != 10)
            throw new InvalidOperationException("INVALID_ANSWER_COUNT");

        var optionIds = answers.Select(a => a.OptionId).ToList();
        var options = await _context.RiskOptions
            .Where(o => optionIds.Contains(o.Id))
            .ToListAsync(cancellationToken);

        if (options.Count != 10)
            throw new InvalidOperationException("INVALID_OPTIONS");

        var totalScore = options.Sum(o => o.Score);
        var riskLevel = totalScore switch
        {
            <= 18 => RiskLevel.Conservative,
            <= 28 => RiskLevel.Balanced,
            _     => RiskLevel.Aggressive
        };

        // ML tahmini — finansal profil ve yaş gerekli
        RiskLevel? mlRiskLevel = null;
        var user = await _context.Users
            .Include(u => u.FinancialProfile)
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user?.FinancialProfile is { } fp && fp.MonthlyIncome > 0)
        {
            var age = DateTime.UtcNow.Year - user.DateOfBirth.Year;
            var questionScores = answers
                .OrderBy(a => a.QuestionId)
                .Select(a => options.First(o => o.Id == a.OptionId).Score)
                .ToList();

            mlRiskLevel = await _mlRiskService.PredictAsync(
                questionScores,
                savingsRate:    (double)(fp.SavingsAmount / fp.MonthlyIncome),
                debtToIncome:   (double)(fp.DebtAmount / fp.MonthlyIncome),
                expenseRatio:   (double)(fp.MonthlyExpenses / fp.MonthlyIncome),
                age:            age,
                cancellationToken);
        }

        var result = new RiskTestResult
        {
            Id           = Guid.NewGuid(),
            UserId       = command.UserId,
            TotalScore   = totalScore,
            RiskLevel    = riskLevel,
            MLRiskLevel  = mlRiskLevel,
            CompletedAt  = DateTime.UtcNow
        };

        _context.RiskTestResults.Add(result);

        var testAnswers = answers.Select(a => new RiskTestAnswer
        {
            Id               = Guid.NewGuid(),
            RiskTestResultId = result.Id,
            RiskQuestionId   = a.QuestionId,
            RiskOptionId     = a.OptionId
        }).ToList();

        _context.RiskTestAnswers.AddRange(testAnswers);
        await _context.SaveChangesAsync(cancellationToken);

        return new RiskTestResultResponse(result.Id, result.TotalScore, result.RiskLevel, result.MLRiskLevel, result.CompletedAt);
    }
}
