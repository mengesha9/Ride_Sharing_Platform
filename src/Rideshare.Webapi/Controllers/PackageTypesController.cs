
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.WebApi.Services;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.PackageTypes.CQRS.Requests.Commands;
using Rideshare.Application.Features.PackageTypes.Dtos;
using Rideshare.Application.Features.PackageTypes.CQRS.Requests.Queries;
using RideShare.Application.Features.PackageTypes.Dtos;
using RideShare.Application.Features.PackageTypes.CQRS.Requests.Queries;

namespace Rideshare.WebApi.Controllers;
[ApiController]
[Route("api/PackageTypes")]

public class PackageTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PackageTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetPackageTypes()
    {
        var response = await _mediator.Send(new GetPackageTypesRequest());
        if (response.Succeeded)
        {
            return Ok(response.Value);
        }
        return BadRequest(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPackageTypeById(int id)
    {
        var response = await _mediator.Send(new GetPackageTypeByIdRequest { Id = id });
        if (response.Succeeded)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPost]
    public async Task<ActionResult> AddPackageType([FromBody] CreatePackageTypeCommand packageType)
    {
        var response = await _mediator.Send(packageType);
        if (response.Succeeded)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdatePackageTypeCommand packageType)
    {
        var response = await _mediator.Send(packageType);
        if (response.Succeeded)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var response = await _mediator.Send(new DeletePackageTypeCommand { Id = id });
        if (!response.Succeeded)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    [HttpGet("packagePrice")]
    public async Task<ActionResult> GetPackagePrice([FromQuery] GetListOfPackagePriceRequestDto request)
    {
        var response = await _mediator.Send(new GetListOfPackagePriceRequst { GetListOfPackagePriceReqeustDto = request });
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}