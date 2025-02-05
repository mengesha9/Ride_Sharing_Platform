using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests;

public class GetPackageRequest : IRequest<BaseResponse<GetPackageResponseDto>>
{
    public GetPackageRequestDto GetPackageRequestDto {set; get;}
}
