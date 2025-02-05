using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Drivers.DTOs;

namespace Rideshare.Application.Features.Drivers.CQRS.Commands
{
    public class VerifyDriverCommand : IRequest<BaseResponse<VerifyDriverResponseDto>> {
        public required VerifyDriverDto verifyDriverDto {get; set;}
    }
}