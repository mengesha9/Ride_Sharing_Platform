using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class RiderHistory : BaseEntity
{
    public ObjectId PackageId { set; get; }
    public ObjectId RiderId { set; get; }
    public required string Name { set; get; }
    public required Location PickUpLocation { set; get; }
    public required Location DropOffLocation { set; get; }
    public required double Distance { set; get; }
    public DateTime StartDateTime { set; get; }
    public double Price { set; get; }
    public PackageType PackageType { set; get; }
    public VehicleType VehicleType { set; get; }
    // public int RatingId {set; get;}
    // public string PaymentType {set; get;}
    // public string TransactionId {set; get;}
    // public double Distance {set; get;}

}
