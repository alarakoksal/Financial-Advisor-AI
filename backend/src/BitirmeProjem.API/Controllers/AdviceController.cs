using System.Security.Claims;
using BitirmeProjem.Application.Features.Advice;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjem.API.Controllers;

[ApiController]
[Route("api/advice")]
[Authorize]
public class AdviceController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdviceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAdvice()
    {
        var advice = await _mediator.Send(new GetFinancialAdviceQuery(GetUserId()));
        return Ok(new { advice });
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var history = await _mediator.Send(new GetAdviceHistoryQuery(GetUserId()));
        return Ok(history);
    }
}
