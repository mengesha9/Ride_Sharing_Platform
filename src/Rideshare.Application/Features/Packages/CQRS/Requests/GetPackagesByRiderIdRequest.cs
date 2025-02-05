using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests;

public class GetPackagesByRiderIdRequest  : IRequest<BaseResponse<List<GetPackagesByRiderIdResponseDto>>>
{
    public GetPackagesByRiderIdRequestDto GetPackagesByRiderIdRequestDto {set; get;}
}
