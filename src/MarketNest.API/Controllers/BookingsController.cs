using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketNest.Application.Bookings;
using MarketNest.Application.Bookings.Queries;

[ApiController]
[Route("api/v1/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingCommand cmd)
    {
        var res = await _mediator.Send(cmd);
        return Created(string.Empty, res);
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> My([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        // current user id will be resolved in handler by ICurrentUserService in future; for now accept userId from claims
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);
        var res = await _mediator.Send(new GetUserBookingsQuery(userId, page, pageSize));
        return Ok(res);
    }

    [Authorize]
    [HttpGet("vendor")]
    public async Task<IActionResult> Vendor([FromQuery] Guid vendorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var res = await _mediator.Send(new GetVendorBookingsQuery(vendorId, page, pageSize));
        return Ok(res);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var res = await _mediator.Send(new MarketNest.Application.Bookings.Queries.GetBookingByIdQuery(id));
        return Ok(res);
    }

    [Authorize(Policy = "VendorOnly")]
    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm([FromRoute] Guid id)
    {
        var res = await _mediator.Send(new ConfirmBookingCommand(id));
        return Ok(res);
    }

    [Authorize]
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, [FromBody] MarketNest.Application.Bookings.CancelBookingCommand cmd)
    {
        var command = new MarketNest.Application.Bookings.CancelBookingCommand(id, cmd.Reason);
        var res = await _mediator.Send(command);
        return Ok(res);
    }

    [Authorize(Policy = "VendorOnly")]
    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete([FromRoute] Guid id)
    {
        var res = await _mediator.Send(new MarketNest.Application.Bookings.CompleteBookingCommand(id));
        return Ok(res);
    }

    [Authorize(Policy = "VendorOnly")]
    [HttpGet("export/ical")]
    public async Task<IActionResult> Export([FromQuery] Guid vendorId, [FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var res = await _mediator.Send(new MarketNest.Application.Bookings.ExportCalendarQuery(vendorId, from, to));
        return File(System.Text.Encoding.UTF8.GetBytes(res), "text/calendar", "bookings.ics");
    }
}
