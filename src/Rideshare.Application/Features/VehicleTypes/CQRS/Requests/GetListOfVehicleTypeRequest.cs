using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Domain.Common;
using RideShare.Application.Features.VehicleTypes.Dtos;

namespace Rideshare.Application.Features.VehicleTypes.CQRS.Requests;

public class GetListOfVehiclePriceRequest : IRequest<BaseResponse<Dictionary<VehicleType, Dictionary<string, double>>>>
{
  public GetListOfVehiclePriceDto GetListOfVehiclePriceDto { get; set; } = null!;
}
