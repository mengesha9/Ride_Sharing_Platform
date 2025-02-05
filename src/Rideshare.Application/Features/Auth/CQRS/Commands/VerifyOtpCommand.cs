using MediatR;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Commands;

public class VerifyOtpCommand : IRequest<BaseCommandResponse<VerifyOtpResponse>>
{
  public VerifyOtpRequest VerifyOtpDto { get; set; } = null!;
}

