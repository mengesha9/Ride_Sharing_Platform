using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Drivers.DTOs;

namespace Rideshare.Application.Features.Drivers.CQRS.Queries
{
    public class GetDriversQuery : IRequest<BaseResponse<List<GetDriversResponseDto>>>
    {
        public required GetDriversQueryDto getDriversQueryDto {get; set;}
    }
}