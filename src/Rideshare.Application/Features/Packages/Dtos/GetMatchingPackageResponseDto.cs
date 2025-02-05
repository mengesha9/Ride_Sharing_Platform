
using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Packages.Dtos;

public class GetMatchingPackageResponseDto
{
        public ObjectId Id {set; get;}
        public string Name {set; get;}
        public double Price{set; get;}
        public PackageType PackageTypeType {set; get;}
        public VehicleType VehicleType {set; get;}
        public Location PickUpLocation {set; get;}
        public Location DropOffLocation {set; get;}
        public DateTime StartDateTime {set; get;}
        public string TotalSeats {set; get;}
        public string AvailableSeats {set; get;}
}
