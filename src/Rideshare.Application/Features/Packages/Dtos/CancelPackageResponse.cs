using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Packages.Dtos;
public class CancelPackageResponse
{
  public ObjectId Id { set; get; }
  public PackageType PackageType { set; get; }
  public VehicleType VehicleType { set; get; }
  public Location PickUpLocation { set; get; } = null!;
  public Location DropOffLocation { set; get; } = null!;
  public DateTime StartDateTime { set; get; }
}