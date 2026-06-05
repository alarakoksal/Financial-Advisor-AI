using System.Security.Claims;
using BitirmeProjem.Application.Features.RiskTest.Commands;
using BitirmeProjem.Application.Features.RiskTest.DTOs;
using BitirmeProjem.Application.Features.RiskTest.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjem.API.Controllers;

[ApiController]
[Route("api/risk-test")]
[Authorize]
public class RiskTestController : ControllerBase
{
    private readonly IMediator _mediator;

    public RiskTestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("questions")]
    public async Task<IActionResult> GetQuestions()
    {
        var result = await _mediator.Send(new GetRiskQuestionsQuery());
        return Ok(result);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitRiskTestRequest request)
    {
        var result = await _mediator.Send(new SubmitRiskTestCommand(GetUserId(), request));
        return Ok(result);
    }

    [HttpGet("result/latest")]
    public async Task<IActionResult> GetLatestResult()
    {
        var result = await _mediator.Send(new GetLatestRiskTestResultQuery(GetUserId()));
        return Ok(result);
    }
}
