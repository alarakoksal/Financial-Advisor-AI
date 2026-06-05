using System.Security.Claims;
using BitirmeProjem.Application.Features.FinancialProfile.Commands;
using BitirmeProjem.Application.Features.FinancialProfile.DTOs;
using BitirmeProjem.Application.Features.FinancialProfile.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjem.API.Controllers;

[ApiController]
[Route("api/financial-profile")]
[Authorize]
public class FinancialProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public FinancialProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetFinancialProfileQuery(GetUserId()));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFinancialProfileRequest request)
    {
        var result = await _mediator.Send(new CreateFinancialProfileCommand(GetUserId(), request));
        return CreatedAtAction(nameof(Get), result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateFinancialProfileRequest request)
    {
        var result = await _mediator.Send(new UpdateFinancialProfileCommand(GetUserId(), request));
        return Ok(result);
    }
}
