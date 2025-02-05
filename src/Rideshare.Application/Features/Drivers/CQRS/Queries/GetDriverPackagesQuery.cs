using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Drivers.DTOs;

namespace Rideshare.Application.Features.Drivers.CQRS.Queries
{
    public class GetDriverPackagesQuery : IRequest<BaseResponse<List<GetDriverPackagesResponseDto>>>
    {
        public required GetDriverPackagesQueryDto getDriverPackagesQueryDto {get; set;}
    }
}