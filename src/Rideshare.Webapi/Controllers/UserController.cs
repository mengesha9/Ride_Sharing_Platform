using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.PackageTypes.CQRS.Requests;
using Rideshare.Application.Features.VehicleTypes.CQRS.Requests;
using Rideshare.Domain.Common;
using Rideshare.WebApi.Services;
using RideShare.Application.Features.VehicleTypes.Dtos;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/User")]

public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserAccessor _userAccessor;

    public UserController(IMediator mediator, IUserAccessor userAccessor)
    {
        _mediator = mediator;
        _userAccessor = userAccessor;
    }

    [HttpGet("GetVehiclePrices")]
    public async Task<ActionResult<BaseResponse<Dictionary<VehicleType, int>>>> GetVehiclePrices([FromQuery] GetListOfVehiclePriceDto request)
    {
        var query = new GetListOfVehiclePriceRequest { GetListOfVehiclePriceDto = request };
        var response = await _mediator.Send(query);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("GetPackageTypesMappings")]
    public async Task<ActionResult<BaseResponse<Dictionary<PackageType, int>>>> GetPackageTypesMapping()
    {
        var response = await _mediator.Send(new GetPackageTypesRequest());
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

}
