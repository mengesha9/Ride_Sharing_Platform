using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Drivers.DTOs;

namespace Rideshare.Application.Features.Drivers.CQRS.Commands
{
    public class DeleteDriverCommand : IRequest<BaseResponse<DeleteDriverResponseDto>> {
        public required DeleteDriverDto deleteDriverDto {get; set;}
    }
}