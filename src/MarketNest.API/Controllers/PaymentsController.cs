using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketNest.Application.Payments;
using MarketNest.Application.Payments.Queries;

[ApiController]
[Route("api/v1/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost("create-intent")]
    public async Task<IActionResult> CreateIntent([FromBody] CreatePaymentIntentCommand cmd)
    {
        var res = await _mediator.Send(cmd);
        return Ok(res);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("{id:guid}/refund")]
    public async Task<IActionResult> Refund([FromRoute] Guid id)
    {
        await _mediator.Send(new ProcessRefundCommand(id));
        return NoContent();
    }

    [Authorize(Policy = "VendorOnly")]
    [HttpGet("vendor/history")]
    public async Task<IActionResult> VendorHistory([FromQuery] Guid vendorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var res = await _mediator.Send(new GetVendorPaymentHistoryQuery(vendorId, page, pageSize));
        return Ok(res);
    }
}
