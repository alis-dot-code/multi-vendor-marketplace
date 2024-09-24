using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketNest.Application.Disputes;

namespace MarketNest.API.Controllers
{
    [ApiController]
    [Route("api/v1/disputes")]
    public class DisputesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DisputesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Open([FromBody] OpenDispute.Command cmd)
        {
            var res = await _mediator.Send(cmd);
            if (!res.IsSuccess) return BadRequest(res.Error);
            return CreatedAtAction(nameof(Get), new { id = res.Value.Id }, res.Value);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            // minimal: return dispute from unit of work via mediator query if implemented
            return NotFound();
        }

        [HttpPost("{id}/resolve")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Resolve(Guid id, [FromBody] ResolveDispute.Command cmd)
        {
            if (id != cmd.DisputeId) return BadRequest();
            var res = await _mediator.Send(cmd);
            if (!res.IsSuccess) return BadRequest(res.Error);
            return Ok();
        }
    }
}
