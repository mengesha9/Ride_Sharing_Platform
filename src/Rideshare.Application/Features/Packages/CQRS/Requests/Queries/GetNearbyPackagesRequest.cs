using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests.Queries
{
    public class GetNearbyPackagesRequest : IRequest<BaseResponse<List<GetNearByPackageWithDistanceResponseDto>>>
    {
        public GetNearbyPackagesDto GetNearbyPackagesDto { get; set; } = null!;
    }
}