using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Auth.CQRS.Commands;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.CQRS.Requests.Commands;
using Rideshare.Application.Features.Packages.CQRS.Requests.Queries;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.Application.Features.Payment.CQRS.Requests.Queries;
using Rideshare.Application.Features.Payment.Dtos;
using Rideshare.Application.Features.RiderLocations.CQRS.Requests;
using Rideshare.Application.Features.RiderLocations.CQRS.Requests.Handlers;
using Rideshare.Application.Features.RiderLocations.Dtos;
using Rideshare.Application.Features.Riders.CQRS.Requests.Commands;
using Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Application.Features.Riders.Requests.Commands;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.WebApi.Services;

namespace Rideshare.WebApi.Controllers;

[ApiController]
[Route("api/Rider")]
[Authorize(Policy = "RiderPolicy")]
public class RiderController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly IUserAccessor _userAccessor;

  public RiderController(IMediator mediator, IUserAccessor userAccessor)
  {
    _mediator = mediator;
    _userAccessor = userAccessor;
  }

  [HttpPut("UpdateDeviceToken")]
  public async Task<ActionResult<VerifyOtpResponse>> UpdateDeviceToken([FromBody] UpdateRiderDeviceTokenRequestDto request)
  {
    var userId = _userAccessor.GetUserId();
    request.RiderId = userId;
    var response = await _mediator.Send(new UpdateRiderDeviceTokenRequest { UpdateRiderDeviceTokenRequestDto = request });
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  [HttpGet("GetProfile")]
  public async Task<ActionResult<VerifyOtpResponse>> GetProfile()
  {
    var riderId = _userAccessor.GetUserId();
    var response = await _mediator.Send(new GetRiderProfileRequest { GetRiderProfileRequestDto = new GetRiderProfileRequestDto { Id = riderId } });
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  // [Authorize(Policy = "RiderPolicy")]
  [HttpGet("GetMatchingPackages")]
  public async Task<ActionResult<BaseResponse<List<GetMatchingPackageResponseDto>>>> GetMatchingPackages(
      [FromQuery] Location pickUpLocation,
      [FromQuery] Location dropOffLocation,
      [FromQuery] double dropOffLongitude,
      [FromQuery] DateTime startDateTime,
      [FromQuery] int vehicleType,
      [FromQuery] int packageType)
  {
    var request = new GetMatchingPackageRequestDto
    {
      PickUpLocation = pickUpLocation,
      DropOffLocation = dropOffLocation,
      StartDateTime = startDateTime,
      VehicleType = (VehicleType)vehicleType,
      PackageType = (PackageType)packageType
    };
    var response = await _mediator.Send(new GetMatchingPackageRequest { GetMatchingPackageRequestDto = request });
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  // [Authorize(Policy = "RiderPolicy")]
  [HttpGet("GetPackage/{packageId}")]
  public async Task<ActionResult<BaseResponse<GetPackageResponseDto>>> GetPackage(string packageId)
  {
    var response = await _mediator.Send(new GetPackageRequest { GetPackageRequestDto = new GetPackageRequestDto { Id = ObjectId.Parse(packageId) } });
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  // [Authorize(Policy = "RiderPolicy")]
  [HttpGet("GetRiderPackages")]
  public async Task<ActionResult<BaseResponse<List<GetPackagesByRiderIdResponseDto>>>> GetSubscribedPackages(int page = 1, int pageSize = 10)
  {
    var riderId = _userAccessor.GetUserId();
    if (riderId == ObjectId.Empty)
    {
      return Unauthorized();
    }
    var response = await _mediator.Send(new GetPackagesByRiderIdRequest { GetPackagesByRiderIdRequestDto = new GetPackagesByRiderIdRequestDto { RiderId = riderId } });
    var totalCount = response.Value.Count;
    var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
    var packagesPerPage = response.Value.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    response.Value = packagesPerPage;

    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  [Authorize("RiderPolicy")]
  [HttpGet("GetRiderActiveCommute")]
  public async Task<ActionResult<BaseResponse<List<GetRiderProfileResponseDto>>>> GetRiderActivePackages(int page = 1, int pageSize = 10)
  {
    var riderId = _userAccessor.GetUserId();
    if (riderId == ObjectId.Empty)
    {
      return Unauthorized();
    }
    var response = await _mediator.Send(new GetActivePackagesByRiderIdRequest { GetActivePackagesByRiderIdRequestDto = new GetActivePackagesByRiderIdRequestDto { RiderId = riderId } });
    var totalCount = response.Value.Count;
    var packagesPerPage = response.Value.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    response.Value = packagesPerPage;

    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  // [Authorize(Policy = "RiderPolicy")]
  [HttpPost("RequestPackage")]
  public async Task<ActionResult<BaseCommandResponse>> SaveLocation([FromBody] CreatePackageRequestDto request)
  {
    var riderId = _userAccessor.GetUserId();
    if (riderId == ObjectId.Empty)
    {
      return Unauthorized();
    }
    var response = await _mediator.Send(new CreatePackageRequest { CreatePackageRequestDto = request, RiderId = riderId });
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  // [Authorize(Policy = "RiderPolicy")]
  [HttpGet("GetRiderLocations")]
  public async Task<ActionResult<BaseResponse<List<GetRiderLocationsResponseDto>>>> GetSavedLocations()
  {
    var riderId = _userAccessor.GetUserId();
    if (riderId == ObjectId.Empty)
    {
      return Unauthorized();
    }
    var response = await _mediator.Send(new GetRiderLocationsByRiderIdRequest { GetRiderLocationsRequestDto = new GetRiderLocationsRequestDto { RiderId = riderId } });
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  // [Authorize(Policy = "RiderPolicy")]
  [HttpPost("SaveRiderLocation")]
  public async Task<ActionResult<BaseCommandResponse>> SaveLocation([FromBody] CreateRiderLocationRequestDto request)
  {
    var riderId = _userAccessor.GetUserId();
    if (riderId == ObjectId.Empty)
    {
      return Unauthorized();
    }
    request.RiderId = riderId;
    var response = await _mediator.Send(new CreateRiderLocationRequest { CreateRiderLocationRequestDto = request });
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  [HttpPatch("UpdateRiderLocation")]
  public async Task<ActionResult<BaseCommandResponse>> UpdateRiderLocation([FromBody] UpdateRiderLocationRequestDto request)
  {
    var riderId = _userAccessor.GetUserId();
    if (riderId == ObjectId.Empty)
    {
      return Unauthorized();
    }
    var response = await _mediator.Send(new UpdateRiderLocationRequest { Id = ObjectId.Parse(request.Id), RiderId = riderId, Name = request.Name, Location = request.Location });
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  // [Authorize(Policy = "RiderPolicy")]
  [HttpPut("UpdateProfile")]
  public async Task<ActionResult<BaseCommandResponse>> UpdateProfile([FromBody] UpdateRiderDto request)
  {
    var riderId = _userAccessor.GetUserId();
    if (riderId == ObjectId.Empty)
    {
      return Unauthorized();
    }
    var response = await _mediator.Send(new UpdateRiderProfileRequest
    {
      LastName = request.LastName,
      FirstName = request.FirstName,
      Email = request.Email,
      PhoneNumber = request.PhoneNumber,
      riderID = riderId
    });
    if (response.Succeeded)
    {
      return Ok(response);
    }
    return BadRequest(response.Errors);
  }



  // [Authorize(Policy = "RiderPolicy")]
  [HttpPut("UpdateProfilePicture")]
  public async Task<ActionResult<BaseCommandResponse>> UpdateRiderProfilePicture([FromForm] UpdateRiderProfilePictureDto request)
  {
    var riderId = _userAccessor.GetUserId();
    if (riderId == ObjectId.Empty)
    {
      return Unauthorized();
    }
    var response = await _mediator.Send(new UpdateRiderProfilePictureRequest
    {
      Image = request.Image,
      riderID = riderId
    });
    if (response.Succeeded)
    {
      return Ok(response);
    }
    return BadRequest(response.Errors);
  }

  // [Authorize(Policy = "RiderPolicy")]
  // [HttpPut("UpdateProfile")]
  // public async Task<ActionResult<BaseCommandResponse>> UpdateProfile( UpdateRiderDto request)
  // {


  //     var response = await _mediator.Send(new UpdateRiderProfileRequest {
  //                                                 LastName = request.LastName,
  //                                                 FirstName=request.FirstName,
  //                                                 Password=request.Password,
  //                                                 Email=request.Email,
  //                                                 ProfilePicture=request.ProfilePicture,
  //                                                 PhoneNumber=request.PhoneNumber
  //                                              });
  //     if (response.IsSuccess)
  //     {
  //         return Ok(response);
  //     }
  //     return BadRequest(response.Errors);
  // }


  [HttpGet("GetRiderHistory")]
  public async Task<ActionResult<BaseResponse<List<GetRiderHistoryResponseDto>>>> GetRiderHistory([FromQuery] GetRiderHistoryRequestDto request)
  {
    var riderId = _userAccessor.GetUserId();
    var response = await _mediator.Send(new GetRiderHistoryRequest
    {
      GetRiderHistoryRequestDto = new GetRiderHistoryRequestDto
      {
        RiderId = riderId,
        SortField = request.SortField,
        IsAscending = request.IsAscending,
        MinPrice = request.MinPrice,
        MaxPrice = request.MaxPrice,
        StartDate = request.StartDate,
        EndDate = request.EndDate
      }
    });
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  [HttpGet("UpdateEmailNotificationPreference")]
  public async Task<ActionResult<BaseCommandResponse>> UpdateEmailNotificationPreference()
  {
    var riderId = _userAccessor.GetUserId();
    var response = await _mediator.Send(new UpdateNotificationPreferenceRequest
    {
      UpdateNotificationPreferenceRequestDto = new UpdateNotificationPreferenceRequestDto { RiderId = riderId }
    }
    );
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

  [HttpGet("pending")]
  public async Task<ActionResult<BaseResponse<Unit>>> GetPendingPackages()
  {
    var riderId = _userAccessor.GetUserId();
    var response = await _mediator.Send(new GetPendingPackagesByIdRequest { GetPendingPackagesByIdRequestDto = new GetPendingPackagesByIdRequestDto { RiderId = riderId } });
    if (response.IsSuccess)
    {
      return Ok(response);
    }

    return BadRequest(response);
  }

  [HttpPatch("cancel/{packageId}")]
  public async Task<ActionResult<BaseResponse<Unit>>> CancelPackage(string packageId)
  {
    var riderId = _userAccessor.GetUserId();

    // Validate packageId format
    if (!ObjectId.TryParse(packageId, out ObjectId parsedPackageId))
    {
      return BadRequest(new BaseResponse<Unit> { IsSuccess = false, Message = "Invalid package ID format" });
    }

    var response = await _mediator.Send(new CancelPackageCommand
    {
      CancelPackageRequestDto = new CancelPackageRequestDto
      {
        RiderId = riderId,
        PackageId = parsedPackageId
      }
    });

    if (response.IsSuccess)
    {
      return Ok(response);
    }

    return BadRequest(response);
  }


  [HttpGet("transaction-history")]
  public async Task<ActionResult<BaseResponse<Unit>>> GetTransactionHistory()
  {
    var riderId = _userAccessor.GetUserId();
    var response = await _mediator.Send(new GetTransactionHistoryRequest { GetTransactionHistoryRequestDto = new GetTransactionHistoryRequestDto { RiderId = riderId } });
    if (response.IsSuccess)
    {
      return Ok(response);
    }

    return BadRequest(response);
  }

  [HttpGet("GetNearByPackages")]
  public async Task<ActionResult<List<PackageDto>>> GetNearbyPackages([FromQuery] GetNearbyPackagesDto getNearbyPackagesDto)
  {
    var query = new GetNearbyPackagesRequest { GetNearbyPackagesDto = getNearbyPackagesDto };
    var response = await _mediator.Send(query);
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);

  }
  [HttpGet("JoinPackage/{packageId}")]
  public async Task<ActionResult<BaseCommandResponse>> JoinPackage(string packageId)
  {
    var riderId = _userAccessor.GetUserId();
    if (riderId == ObjectId.Empty)
    {
      return Unauthorized();
    }
    var response = await _mediator.Send(new JoinPackageRequest { JoinPackageRequestDto = new JoinPackageRequestDto { RiderId = riderId, PackageId = packageId } });
    if (response.IsSuccess)
    {
      return Ok(response);
    }
    return BadRequest(response);
  }

}
