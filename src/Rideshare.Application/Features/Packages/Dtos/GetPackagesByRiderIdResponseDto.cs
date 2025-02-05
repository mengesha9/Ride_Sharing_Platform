using MongoDB.Bson;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Packages.Dtos;

public class GetPackagesByRiderIdResponseDto : IPackageDto
{
    public ObjectId Id { set; get; }
    public string Name { set; get; }
    public double Price { set; get; }
    public PackageType PackageType { set; get; }
    public VehicleType VehicleType { set; get; }
    public Location PickUpLocation { set; get; }
    public Location DropOffLocation { set; get; }
    public int TotalSeats { set; get; }
    public int AvailableSeats { set; get; }
    public bool IsValid { set; get; }
    public ObjectId AssignedDriver { set; get; } = ObjectId.Empty;
    public List<Rider> RegisteredRiders { get; set; } = new List<Rider>();

}
