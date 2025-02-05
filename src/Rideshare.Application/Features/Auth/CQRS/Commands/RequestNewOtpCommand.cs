using MediatR;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Commands
{
    public class RequestNewOtpCommand : IRequest<BaseCommandResponse<RequestOtpResponseDto>>
    {
        public RequestOtpDto RequestOtpDto { get; set; } = null!;
    }
}