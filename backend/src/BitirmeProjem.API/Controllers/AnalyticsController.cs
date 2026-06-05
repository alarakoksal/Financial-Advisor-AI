using System.Security.Claims;
using BitirmeProjem.Application.Features.Analytics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjem.API.Controllers;

[ApiController]
[Route("api/analytics")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnalyticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("financial-health")]
    public async Task<IActionResult> GetFinancialHealth()
    {
        var result = await _mediator.Send(new GetFinancialHealthQuery(GetUserId()));
        return Ok(result);
    }
}
