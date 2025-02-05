using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class RiderLocation : BaseEntity
{
     public ObjectId RiderId {set; get;}
     public string Name {set; get;}
     public Location Location {set; get;}
     
}
