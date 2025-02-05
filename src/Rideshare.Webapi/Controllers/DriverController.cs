using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Auth.CQRS.Commands;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Features.Drivers.CQRS.Queries;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Features.DriverHistory.Dtos;
using Rideshare.Application.Features.Drivers.CQRS.Commands;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.Application.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rideshare.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DriverController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("update")]
        public async Task<ActionResult<BaseCommandResponse<UpdateDriverInformationResponseDto>>> Update([FromForm] UpdateDriverInformationDto updateDriverInformationDto)
        {
            var command = new UpdateDriverInformationCommand { UpdateDriverInformationDto = updateDriverInformationDto };
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpPost("{driverId}/preferred-packages/{packageId}")]
        public async Task<IActionResult> AddPreferredPackageToDriver(string driverId, string packageId)
        {
            if (!ObjectId.TryParse(driverId, out var driverObjectId) || !ObjectId.TryParse(packageId, out var packageObjectId))
            {
                return BadRequest("Invalid ObjectId format for driverId or packageId.");
            }

            var command = new CreatePreferedPackageRequest { CreatePreferedPackageRequestDto = new CreatePreferedPackageRequestDto { DriverId = driverObjectId, PackageId = packageObjectId } };
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return CreatedAtAction(nameof(AddPreferredPackageToDriver), response);
            }
            return BadRequest(response);
        }

        [HttpGet("{driverId}/preferred-packages")]
        public async Task<IActionResult> GetPreferredPackagesForDriver(string driverId)
        {
            if (!ObjectId.TryParse(driverId, out var driverObjectId))
            {
                return BadRequest("Invalid ObjectId format for driverId.");
            }

            var query = new GetPreferredPackagesQuery { DriverId = driverObjectId };
            var preferredPackages = await _mediator.Send(query);
            return Ok(preferredPackages);
        }
        // a controller to get unassigned packages
        [HttpGet("unassigned-packages")]
        public async Task<IActionResult> GetUnassignedPackages()
        {
            var request = new GetUnassignedPackagesRequest { GetUnassignedPackagesRequestDto = new GetUnassignedPackagesRequestDto() };
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpGet("History/{driverId}/{pageNumber}")]
        public async Task<ActionResult<BaseResponse<List<GetDriverHIstoryAndEarningsResponseDto>>>> GetHistory(string driverId, int pageNumber = 1)
        {
            var request = new GetDriverHistoryAndEarningsRequest { GetDriverHIstoryAndEarningsRequestDto = new GetDriverHIstoryAndEarningsRequestDto { DriverId = driverId, PageNumber = pageNumber } };
            var response = await _mediator.Send(request);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }


        [HttpGet("{pageSize}/{pageNumber}")]
        public async Task<ActionResult<BaseResponse<List<GetDriversResponseDto>>>> GetDrivers(int pageNumber = 1, int pageSize = 10)
        {
            var request = new GetDriversQuery
            {
                getDriversQueryDto = new GetDriversQueryDto
                {
                    pageNumber = pageNumber,
                    pageSize = pageSize
                }
            };

            var response = await _mediator.Send(request);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("verify")]
        public async Task<ActionResult<BaseResponse<VerifyDriverResponseDto>>> VerifyDriver([FromBody] VerifyDriverDto verifyDriverDto)
        {
            var command = new VerifyDriverCommand { verifyDriverDto = verifyDriverDto };
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        [HttpDelete("{driverId}")]
        public async Task<ActionResult<BaseResponse<DeleteDriverResponseDto>>> DeleteDriver(string driverId)
        {
            var command = new DeleteDriverCommand { deleteDriverDto = new DeleteDriverDto { driverId = driverId } };
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                return NotFound(response);
            }
        }

        [HttpPost("search")]
        public async Task<ActionResult<BaseResponse<List<SearchDriversResponseDto>>>> SearchDrivers([FromBody] SearchDriversQueryDto searchDriversQueryDto)
        {
            var query = new SearchDriversQuery { searchDriversQueryDto = searchDriversQueryDto };
            var response = await _mediator.Send(query);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpPost("packages/{driverId}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<BaseResponse<List<GetDriverPackagesResponseDto>>>> GetPackages(string driverId, int pageNumber = 1, int pageSize = 10)
        {
            var query = new GetDriverPackagesQuery { getDriverPackagesQueryDto = new GetDriverPackagesQueryDto { DriverId = driverId, PageNumber = pageNumber, PageSize = pageSize } };
            var response = await _mediator.Send(query);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
