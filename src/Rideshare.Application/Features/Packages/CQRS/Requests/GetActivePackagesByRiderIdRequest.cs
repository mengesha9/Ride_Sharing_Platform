using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests;

public class GetActivePackagesByRiderIdRequest : IRequest<BaseResponse<List<GetActivePackagesByRiderIdResponseDto>>>
{
  public GetActivePackagesByRiderIdRequestDto GetActivePackagesByRiderIdRequestDto { set; get; }
}