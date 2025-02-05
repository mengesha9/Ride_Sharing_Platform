using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class RiderHistoryWithPackage : BaseEntity
{
    public ObjectId PackageId { set; get; }
    public ObjectId RiderId { set; get; }
    public required Location PickUpLocation { set; get; }
    public required Location DropOffLocation { set; get; }
    public required double Distance { set; get; }

    public DateTime StartDateTime { set; get; }
    public double Price { set; get; }
    public PackageType PackageType { set; get; }
    public required Package Package { set; get; }

}
