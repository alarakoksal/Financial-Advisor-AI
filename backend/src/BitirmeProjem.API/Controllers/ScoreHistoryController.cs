using System.Security.Claims;
using BitirmeProjem.Application.Features.ScoreHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjem.API.Controllers;

[ApiController]
[Route("api/score-history")]
[Authorize]
public class ScoreHistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScoreHistoryController(IMediator mediator) => _mediator = mediator;

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetHistory() =>
        Ok(await _mediator.Send(new GetScoreHistoryQuery(GetUserId())));
}
