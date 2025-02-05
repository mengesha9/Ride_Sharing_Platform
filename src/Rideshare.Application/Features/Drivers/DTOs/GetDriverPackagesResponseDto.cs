using MongoDB.Bson;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Drivers.DTOs
{
    public class GetDriverPackagesResponseDto
    {
        public required string Name { get; set; }
        public double Price { set; get; }
        public PackageType PackageType { set; get; }
        public VehicleType VehicleType { set; get; }
        public required Location PickUpLocation { set; get; }
        public required Location DropOffLocation { set; get; }
        public DateTime StartDateTime { set; get; }
        // public string Description {set; get;}
        // public string TermsAndConditions {set; get;}
        public int TotalSeats { set; get; }
        public int AvailableSeats { set; get; }
        public bool IsValid { set; get; }
        public ObjectId  AssignedDriver { set; get; } = ObjectId.Empty;
        public List<Rider> RegisteredRiders { get; set; } = new List<Rider>();
    }
}