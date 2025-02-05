using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
using Rideshare.Application.Features.Riders.Dtos;

namespace Rideshare.Webapi.Controllers;

[Route("api/[controller]")]
[ApiController]


public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    // get list of registered riders
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("GetRegisteredRiders")]
    public async Task<IActionResult> GetRegisteredRiders(int page = 1, int pageSize = 10)
    {
        var request = new GetRiderListRequest 
        { 
            GetRidersListRequestDto = new GetRidersListRequestDto 
            { 
                PageNumber = page, 
                PageSize = pageSize 
            } 
        };

        var response = await _mediator.Send(request);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        var totalCount = response.Value.Count;
        var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
        var ridersPerPage = response.Value.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        response.Value = ridersPerPage;

        return Ok(new 
        { 
            Data = response.Value, 
            TotalCount = totalCount, 
            PageNumber = page, 
            PageSize = pageSize, 
            TotalPages = totalPages 
        });
    }


    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("GetRiderById/{riderId}")]
    public async Task<IActionResult> GetRiderById(string riderId)
    {
        if (!ObjectId.TryParse(riderId, out var objectId))
        {
            return BadRequest(new { ErrorType = "Failure", ErrorMessage = "Invalid rider ID format. It must be a 24-digit hexadecimal string." });
        }

        var request = new GetRiderByIdRequest { GetRiderByIdRequestDto = new GetRiderByIdRequestDto { RiderId = objectId } };
        var response = await _mediator.Send(request);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

}