using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Features.Waitlists.Dtos;
using RideShare.Application.Features.Waitlists.CQRS.Commands;

namespace RideShare.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitlistController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WaitlistController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddToWaitlist(WaitlistRequestDto waitlistRequestDto)
        {
            var command = new WaitlistCommand { WaitlistRequestDto = waitlistRequestDto };
            await _mediator.Send(command);
            return Ok();
        }
    }
}