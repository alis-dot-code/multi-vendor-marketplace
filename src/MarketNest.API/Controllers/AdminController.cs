using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketNest.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator) => _mediator = mediator;

        [HttpGet("analytics")]
        public async Task<IActionResult> Analytics()
        {
            // placeholder: should call a mediator query to get analytics
            var result = new
            {
                totalGmvCents = 0,
                totalRevenueCents = 0,
                totalBookings = 0,
                activeVendors = 0,
                totalUsers = 0,
                monthlyRevenue = new int[12]
            };
            return Ok(result);
        }
    }
}
