using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests;

public class GetMatchingPackageRequest : IRequest<BaseResponse<List<GetMatchingPackageResponseDto>>>
{
    public GetMatchingPackageRequestDto GetMatchingPackageRequestDto {set; get;}
}
