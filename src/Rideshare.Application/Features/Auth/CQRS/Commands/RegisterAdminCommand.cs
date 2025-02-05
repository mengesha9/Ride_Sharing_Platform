using MediatR;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Commands
{
  public class RegisterAdminCommand : IRequest<BaseCommandResponse<RegisterAdminResponseDto>>
  {
    public RegisterAdminDto RegisterAdminDto { get; set; } = null!;
  }
}
