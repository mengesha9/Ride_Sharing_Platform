using MediatR;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Commands;

public class ResetUserPasswordVerifyCommand : IRequest<BaseCommandResponse<ResetUserPasswordVerifyResponseDto>>
{
  public ResetUserPasswordVerifyDto ResetUserPasswordVerifyDto { get; set; } = null!;
}
