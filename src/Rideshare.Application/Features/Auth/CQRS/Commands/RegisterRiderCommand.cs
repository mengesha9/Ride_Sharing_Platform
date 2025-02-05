using MediatR;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Commands;

public class RegisterRiderCommand : IRequest<BaseCommandResponse<RegisterRiderResponseDto>>
{
  public RegisterRiderDto RegisterRiderDto { get; set; } = null!;
}

