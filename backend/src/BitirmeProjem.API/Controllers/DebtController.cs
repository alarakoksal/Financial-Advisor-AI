using System.Security.Claims;
using BitirmeProjem.Application.Features.Debts.Commands;
using BitirmeProjem.Application.Features.Debts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjem.API.Controllers;

[ApiController]
[Route("api/debts")]
[Authorize]
public class DebtController : ControllerBase
{
    private readonly IMediator _mediator;

    public DebtController(IMediator mediator) => _mediator = mediator;

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _mediator.Send(new GetDebtsQuery(GetUserId())));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DebtRequest request) =>
        Ok(await _mediator.Send(new CreateDebtCommand(GetUserId(), request)));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DebtRequest request) =>
        Ok(await _mediator.Send(new UpdateDebtCommand(GetUserId(), id, request)));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteDebtCommand(GetUserId(), id));
        return NoContent();
    }
}
