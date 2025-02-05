using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Packages.Dtos;

public class GetMatchingPackageRequestDto
{
    public Location PickUpLocation {set; get;}
    public Location DropOffLocation {set; get;}
    public DateTime StartDateTime {set; get;}
    public VehicleType VehicleType {set; get;}
    public PackageType PackageType {set; get;}
}
