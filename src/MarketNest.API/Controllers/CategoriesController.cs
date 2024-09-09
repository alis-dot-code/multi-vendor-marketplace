using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketNest.Application.Categories;

[ApiController]
[Route("api/v1/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        var res = await _mediator.Send(new GetCategoriesQuery());
        return Ok(res);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand cmd)
    {
        var res = await _mediator.Send(cmd);
        return Created(string.Empty, res);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCategoryCommand cmd)
    {
        var command = new UpdateCategoryCommand(id, cmd.Name, cmd.Slug, cmd.Description, cmd.ParentId, cmd.SortOrder, cmd.IsActive);
        var res = await _mediator.Send(command);
        return Ok(res);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }
}
