using Rideshare.Application.Features.Packages.Dtos;

namespace RideShare.Application.Features.VehicleTypes.Dtos;
public class GetListOfVehiclePriceDto
{
  public LocationRequestDto UserLocation { get; set; } = null!;
  public LocationRequestDto Destination { get; set; } = null!;
}