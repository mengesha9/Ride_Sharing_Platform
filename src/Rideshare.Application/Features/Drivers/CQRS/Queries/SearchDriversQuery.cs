using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Drivers.DTOs;

namespace Rideshare.Application.Features.Drivers.CQRS.Queries
{
    public class SearchDriversQuery : IRequest<BaseResponse<List<SearchDriversResponseDto>>>
    {
        public required SearchDriversQueryDto searchDriversQueryDto {get; set;}
    }
}