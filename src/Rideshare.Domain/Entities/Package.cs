using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class Package : BaseEntity
{
  public string Name { get; set; } = null!;
  public double Price { set; get; }
  public PackageType PackageType { set; get; }
  public VehicleType VehicleType { set; get; }
  public Location PickUpLocation { set; get; } = null!;
  public Location DropOffLocation { set; get; } = null!;
  public DateTime StartDateTime { set; get; }
  public int TotalSeats { set; get; }
  public int AvailableSeats { set; get; }
  public bool IsValid { set; get; }
  public bool IsActive { set; get; }
  public ObjectId AssignedDriver { set; get; } = ObjectId.Empty;
  public List<Rider> RegisteredRiders { get; set; } = new List<Rider>();
}
