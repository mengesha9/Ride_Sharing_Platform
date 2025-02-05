using MediatR;
using Rideshare.Application.Features.Payment.Dtos;

namespace Rideshare.Application.Features.Payment.CQRS.Requests.Queries;

public class CalculatePaymentRequest : IRequest<PaymentCalculationResponse>
{
  public required CalculatePaymentDto calculatePaymentDto { get; set; }
}
