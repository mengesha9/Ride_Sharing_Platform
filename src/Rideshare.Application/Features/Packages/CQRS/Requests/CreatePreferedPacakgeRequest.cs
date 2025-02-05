using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests;

public class CreatePreferedPackageRequest : IRequest<BaseResponse<CreatePreferedPackageResponseDto>>
{
    public CreatePreferedPackageRequestDto CreatePreferedPackageRequestDto {get; set;}
}