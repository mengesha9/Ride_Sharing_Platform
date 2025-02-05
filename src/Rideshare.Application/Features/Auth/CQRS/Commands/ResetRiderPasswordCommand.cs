using MediatR;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Commands
{
    public class ResetRiderPasswordCommand : IRequest<BaseCommandResponse<ResetRiderPasswordResponseDto>>
    {
        public ResetRiderPasswordDto ResetRiderPasswordDto { get; set; } = null!;
    }
}