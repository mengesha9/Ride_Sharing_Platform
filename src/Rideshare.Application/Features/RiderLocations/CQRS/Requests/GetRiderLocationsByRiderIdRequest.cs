using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.RiderLocations.Dtos;

namespace Rideshare.Application.Features.RiderLocations.CQRS.Requests.Handlers;
public class GetRiderLocationsByRiderIdRequest : IRequest<BaseResponse<List<GetRiderLocationsResponseDto>>>
{
    public GetRiderLocationsRequestDto GetRiderLocationsRequestDto {set; get;}
}