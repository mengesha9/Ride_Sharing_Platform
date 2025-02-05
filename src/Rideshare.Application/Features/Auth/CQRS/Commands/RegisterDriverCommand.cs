using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Commands
{
    public class RegisterDriverCommand : IRequest<BaseCommandResponse<RegisterDriverResponseDto>>
    {
        public RegisterDriverDto RegisterDriverDto { get; set; } = null!;
    }
}