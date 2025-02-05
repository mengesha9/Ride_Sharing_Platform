using MediatR;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Commands
{
  public class LoginUserCommand : IRequest<BaseCommandResponse<LoginResponseDto>>
  {
    public LoginUserDto LoginUserDto { get; set; } = null!;
  }
}
