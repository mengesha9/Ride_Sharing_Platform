using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Drivers.DTOs;
namespace Rideshare.Application.Features.Drivers.CQRS.Commands
{
    public class UpdateDriverInformationCommand : IRequest<BaseResponse<UpdateDriverInformationResponseDto>> {
        public required UpdateDriverInformationDto UpdateDriverInformationDto {get; set;}
    }
}