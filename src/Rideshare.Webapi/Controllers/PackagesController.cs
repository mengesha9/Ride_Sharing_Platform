using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.CQRS.Requests.Commands;
using Rideshare.Application.Features.Packages.CQRS.Requests.Queries;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.WebApi.Services;

namespace Rideshare.Webapi.Controllers
{


    [ApiController]
    [Route("api/packages")]
    public class PackagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PackagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<ActionResult<BaseResponse<PackageDto>>> GetPackages([FromQuery] GetPackagesDto getPackagesDto)
        {
            var response = await _mediator.Send(new GetPackagesQuery { GetPackagesDto = getPackagesDto });
            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("{packageId}")]
        public async Task<ActionResult<BaseResponse<Unit>>> DeletePackage(string packageId)
        {
            var response = await _mediator.Send(new DeletePackageCommand { DeletePackageDto = new DeletePackageDto { PackageId = new ObjectId(packageId) } });
            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}
