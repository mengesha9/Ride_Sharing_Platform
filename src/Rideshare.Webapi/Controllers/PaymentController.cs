using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Payment.CQRS.Requests.Queries;

namespace Rideshare.WebApi.Controllers;
[ApiController]
[Route("api/")]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;
    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("PaymentCalculate")]
    public async Task<ActionResult<PaymentCalculationResponse>> CalculatePayment(CalculatePaymentRequest request)
    {

        var payment = await _mediator.Send(request);
        if (payment == null)
        {
            return BadRequest();
        }
        return Ok(payment);
    }
}