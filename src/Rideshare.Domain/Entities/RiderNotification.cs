using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rideshare.Domain.Common;
namespace Rideshare.Domain.Entities;

public class RiderNotification : BaseEntity
{
    [BsonRepresentation(BsonType.String)]
    public RiderNotificationType NotificationType { set; get; }
    public List<ObjectId> RiderIds { set; get; }
    public ObjectId packageId { set; get; }
    public string Title { set; get; }
    public string Description { set; get; }
    public Dictionary<string, string> Data { set; get; }
}
