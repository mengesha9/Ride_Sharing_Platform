using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests;

public class GetPendingPackagesByIdRequest  : IRequest<BaseResponse<List<GetPendingPackagesByIdResponseDto>>>
{
    public GetPendingPackagesByIdRequestDto? GetPendingPackagesByIdRequestDto {set; get;}
}
