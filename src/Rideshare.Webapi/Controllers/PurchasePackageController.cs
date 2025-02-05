using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Features.PaymentSystem.Application.Dtos;
using Rideshare.Application.Features.PaymentSystem.CQRS.Handlers;
using Rideshare.Application.Features.PurchasePackage.CQRS.Command;

namespace Rideshare.WebApi.PaymentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IChapaService _chapaService;

    public PaymentController(IChapaService chapaService, IMediator mediator)
    {
        _mediator = mediator;
        _chapaService = chapaService;

    }

    [HttpPost("Process")]
    // [Authorize(Policy = "RiderPolicy")]
    public async Task<IActionResult> ProcessPaymentHandler(ChapaRequestDto requestDTO)
    {

        var response = await _mediator.Send(new ProcessPaymentCommand { RequestDto = requestDTO });
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    //chapa webhook
    // [HttpGet("verify/{txRef}")]
    // public async Task<IActionResult> VerifyPaymentHandler(string txRef)
    // {
    //     var response = await _mediator.Send(new VerifyAsync(txRef);
    //     return Ok(response);
    // }
}