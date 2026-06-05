using BitirmeProjem.Application.Common.Interfaces;
using BitirmeProjem.Application.Features.RiskTest.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjem.Application.Features.RiskTest.Queries;

public record GetRiskQuestionsQuery : IRequest<List<RiskQuestionDto>>;

public class GetRiskQuestionsQueryHandler : IRequestHandler<GetRiskQuestionsQuery, List<RiskQuestionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRiskQuestionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RiskQuestionDto>> Handle(GetRiskQuestionsQuery query, CancellationToken cancellationToken)
    {
        var questions = await _context.RiskQuestions
            .Include(q => q.Options)
            .OrderBy(q => q.OrderIndex)
            .ToListAsync(cancellationToken);

        return questions.Select(q => new RiskQuestionDto(
            q.Id,
            q.OrderIndex,
            q.QuestionTextTr,
            q.QuestionTextEn,
            q.Options.Select(o => new RiskOptionDto(o.Id, o.OptionTextTr, o.OptionTextEn, o.Score)).ToList()
        )).ToList();
    }
}
