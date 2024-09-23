using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketNest.Application.Notifications;

namespace MarketNest.API.Controllers
{
    [ApiController]
    [Route("api/v1/notifications")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendNotification.Command cmd)
        {
            var res = await _mediator.Send(cmd);
            if (!res.IsSuccess) return BadRequest(res.Error);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}
