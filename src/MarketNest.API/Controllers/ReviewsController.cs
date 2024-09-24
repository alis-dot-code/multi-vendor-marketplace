using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketNest.Application.Reviews;
using MarketNest.Application.Reviews.Queries;

namespace MarketNest.API.Controllers
{
    [ApiController]
    [Route("api/v1/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateReview.Command cmd)
        {
            var res = await _mediator.Send(cmd);
            if (!res.IsSuccess) return BadRequest(res.Error);
            return CreatedAtAction(nameof(GetByListing), new { id = res.Value.ListingId }, res.Value);
        }

        [HttpGet("listing/{id}")]
        public async Task<IActionResult> GetByListing(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var res = await _mediator.Send(new GetReviewsByListing.Query(id, page, pageSize));
            return Ok(res);
        }

        [HttpPost("{id}/reply")]
        [Authorize]
        public async Task<IActionResult> Reply(Guid id, [FromBody] ReplyToReview.Command cmd)
        {
            if (id != cmd.ReviewId) return BadRequest();
            var res = await _mediator.Send(cmd);
            if (!res.IsSuccess) return BadRequest(res.Error);
            return Ok(res.Value);
        }

        [HttpPost("{id}/moderate")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Moderate(Guid id, [FromBody] ModerateReview.Command cmd)
        {
            if (id != cmd.ReviewId) return BadRequest();
            var res = await _mediator.Send(cmd);
            if (!res.IsSuccess) return BadRequest(res.Error);
            return Ok();
        }
    }
}
