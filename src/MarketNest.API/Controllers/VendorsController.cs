using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketNest.Application.Vendors.Commands;
using MarketNest.Application.Vendors.Queries;

namespace MarketNest.API.Controllers;

[ApiController]
[Route("api/v1/vendors")]
public class VendorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VendorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost("apply")]
    public async Task<IActionResult> Apply([FromBody] ApplyVendorCommand cmd)
    {
        var res = await _mediator.Send(cmd);
        return Created(string.Empty, res);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("pending")]
    public async Task<IActionResult> Pending()
    {
        var res = await _mediator.Send(new GetPendingVendorsQuery());
        return Ok(res);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve([FromRoute] Guid id, [FromBody] ApproveVendorCommand cmd)
    {
        var command = new ApproveVendorCommand(id, cmd.AdminNotes);
        var res = await _mediator.Send(command);
        return Ok(res);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject([FromRoute] Guid id, [FromBody] RejectVendorCommand cmd)
    {
        var command = new RejectVendorCommand(id, cmd.Reason);
        var res = await _mediator.Send(command);
        return Ok(res);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug([FromRoute] string slug)
    {
        var res = await _mediator.Send(new GetVendorBySlugQuery(slug));
        return Ok(res);
    }

    [Authorize]
    [HttpGet("me/dashboard")]
    public async Task<IActionResult> Dashboard([FromQuery] Guid vendorId)
    {
        var res = await _mediator.Send(new GetVendorDashboardQuery(vendorId));
        return Ok(res);
    }

    [Authorize]
    [HttpPut("me/profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateVendorProfileCommand cmd)
    {
        var res = await _mediator.Send(cmd);
        return Ok(res);
    }
}
