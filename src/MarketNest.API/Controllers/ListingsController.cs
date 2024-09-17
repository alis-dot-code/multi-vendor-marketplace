using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketNest.Application.Listings;
using MarketNest.Application.Common.DTOs;

[ApiController]
[Route("api/v1/listings")]
public class ListingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ListingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? q, [FromQuery] Guid? categoryId, [FromQuery] int? priceMinCents, [FromQuery] int? priceMaxCents, [FromQuery] decimal? minRating, [FromQuery] bool? isVirtual, [FromQuery] string? city, [FromQuery] string? sort, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var sortBy = sort switch
        {
            "price_asc" => MarketNest.Domain.Interfaces.Repositories.ListingSortBy.PriceAsc,
            "price_desc" => MarketNest.Domain.Interfaces.Repositories.ListingSortBy.PriceDesc,
            "rating" => MarketNest.Domain.Interfaces.Repositories.ListingSortBy.Rating,
            "newest" => MarketNest.Domain.Interfaces.Repositories.ListingSortBy.Newest,
            _ => MarketNest.Domain.Interfaces.Repositories.ListingSortBy.Relevance
        };

        var res = await _mediator.Send(new SearchListingsQuery(q, categoryId, priceMinCents, priceMaxCents, minRating, isVirtual, city, sortBy, page, pageSize));
        return Ok(res);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var res = await _mediator.Send(new GetListingByIdQuery(id));
        return Ok(res);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateListingCommand cmd)
    {
        var res = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id = res.Id }, res);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateListingCommand cmd)
    {
        var command = new UpdateListingCommand(id, cmd.Title, cmd.Description, cmd.ShortDescription, cmd.PriceCents, cmd.DurationMinutes, cmd.CategoryId, cmd.IsVirtual, cmd.VirtualLink, cmd.Status);
        var res = await _mediator.Send(command);
        return Ok(res);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteListingCommand(id));
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id:guid}/images")]
    public async Task<IActionResult> UploadImages([FromRoute] Guid id)
    {
        var files = Request.Form.Files;
        var uploads = new List<MarketNest.Application.Common.Models.FileUpload>();
        foreach (var f in files)
        {
            var ms = new MemoryStream();
            await f.CopyToAsync(ms);
            ms.Position = 0;
            uploads.Add(new MarketNest.Application.Common.Models.FileUpload { FileName = f.FileName, Content = ms });
        }
        var res = await _mediator.Send(new UploadListingImagesCommand(id, uploads));
        return Ok(res);
    }

    [Authorize]
    [HttpDelete("{id:guid}/images/{imageId:guid}")]
    public async Task<IActionResult> DeleteImage([FromRoute] Guid id, [FromRoute] Guid imageId)
    {
        await _mediator.Send(new DeleteListingImageCommand(id, imageId));
        return NoContent();
    }

    [HttpGet("{id:guid}/availability")]
    public async Task<IActionResult> GetAvailability([FromRoute] Guid id, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var res = await _mediator.Send(new GetAvailabilitySlotsQuery(id, from, to));
        return Ok(res);
    }

    [Authorize]
    [HttpPost("{id:guid}/availability")]
    public async Task<IActionResult> CreateAvailability([FromRoute] Guid id, [FromBody] MarketNest.Application.Listings.Availability.CreateAvailabilitySlotCommand cmd)
    {
        var command = new MarketNest.Application.Listings.Availability.CreateAvailabilitySlotCommand(id, cmd.StartTime, cmd.EndTime);
        var res = await _mediator.Send(command);
        return Ok(res);
    }

    [Authorize]
    [HttpPut("availability/{slotId:guid}")]
    public async Task<IActionResult> UpdateSlot([FromRoute] Guid slotId, [FromBody] MarketNest.Application.Listings.Availability.UpdateAvailabilitySlotCommand cmd)
    {
        var command = new MarketNest.Application.Listings.Availability.UpdateAvailabilitySlotCommand(slotId, cmd.IsBlocked);
        var res = await _mediator.Send(command);
        return Ok(res);
    }

    [Authorize]
    [HttpDelete("availability/{slotId:guid}")]
    public async Task<IActionResult> DeleteSlot([FromRoute] Guid slotId)
    {
        await _mediator.Send(new MarketNest.Application.Listings.Availability.DeleteAvailabilitySlotCommand(slotId));
        return NoContent();
    }
}
