using MediatR;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Commands;

public class ResetUserPasswordVerifyOtpCommand : IRequest<BaseCommandResponse<ResetUserPasswordVerifyOTPResponseDto>>
{
  public ResetUserPasswordVerifyOTPDto ResetUserPasswordVerifyOtpDTO { get; set; } = null!;
}

