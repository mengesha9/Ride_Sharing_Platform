using MongoDB.Bson;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Packages.Dtos;

public class GetNearByPackageWithDistanceResponseDto : IPackageDto
{
  public DateTime createdAt { get; set; }
  public ObjectId Id { get; set; }
  public string Name { get; set; } = null!;
  public double Price { set; get; }
  public PackageType PackageType { set; get; }
  public VehicleType VehicleType { set; get; }
  public Location PickUpLocation { set; get; } = null!;
  public Location DropOffLocation { set; get; } = null!;
  public DateTime StartDateTime { set; get; }
  public int TotalSeats { set; get; }
  public int AvailableSeats { set; get; }
  public bool IsActive { set; get; }
  public bool IsValid { set; get; }
  public ObjectId AssignedDriver { set; get; } = ObjectId.Empty;
  public List<Rider> RegisteredRiders { get; set; } = new List<Rider>();
  public double Distance { get; set; }
}
