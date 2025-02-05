using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Domain.Common;
using RideShare.Application.Features.PackageTypes.Dtos;

namespace RideShare.Application.Features.PackageTypes.CQRS.Requests.Queries;

public class GetListOfPackagePriceRequst : IRequest<BaseResponse<Dictionary<PackageType, double>>>
{
  public GetListOfPackagePriceRequestDto GetListOfPackagePriceReqeustDto { get; set; }


}