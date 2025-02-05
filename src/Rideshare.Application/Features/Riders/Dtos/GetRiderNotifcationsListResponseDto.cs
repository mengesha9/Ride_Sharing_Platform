using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Riders.Dtos
{
    public class GetRiderNotifcationsListResponseDto
    {
        public ObjectId Id { set; get; }
        [BsonRepresentation(BsonType.String)]
        public RiderNotificationType Type { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public ObjectId packageId { set; get; }
        public Dictionary<string, string> Data { set; get; }
    }
}