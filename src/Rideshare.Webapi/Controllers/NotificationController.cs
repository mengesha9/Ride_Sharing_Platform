using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.WebApi.Services;

namespace Rideshare.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserAccessor _userAccessor;
        public NotificationController(IMediator mediator, IUserAccessor userAccessor)
        {
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        [HttpGet("GetRiderNotifications")]
public async Task<ActionResult<VerifyOtpResponse>> GetRiderNotifications()
{
    var riderId = _userAccessor.GetUserId();
    
    if (riderId == ObjectId.Empty)
    {
        // User is unauthorized, return a 401 Unauthorized response
        return Unauthorized(new { message = "Unauthorized: User is not authenticated." });
    }
    
    var response = await _mediator.Send(new GetRiderNotificationListQuery { GetRiderNotifcationsListRequestDto = new GetRiderNotifcationsListRequestDto { Id = riderId } });
    
    if (response.IsSuccess)
    {
        return Ok(response);
    }
    
    return BadRequest(response);
}

    }
}
