using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Packages.Dtos;

public class CreatePackageRequestDto
{
    public string Name { get; set; }
    public Location PickUpLocation { set; get; }
    public Location DropOffLocation { set; get; }
    public DateTime StartDateTime { set; get; }
    public PackageType PackageType { set; get; }
    public VehicleType VehicleType { set; get; }
}
