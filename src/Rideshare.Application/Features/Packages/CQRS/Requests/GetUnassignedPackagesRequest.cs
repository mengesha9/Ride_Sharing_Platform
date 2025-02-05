using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests;

public class GetUnassignedPackagesRequest  : IRequest<BaseResponse<List<GetUnassignedPackagesResponseDto>>>
{
    public GetUnassignedPackagesRequestDto GetUnassignedPackagesRequestDto {set; get;}
}
