using MongoDB.Bson;

namespace Rideshare.Application.Features.Drivers.DTOs
{
    public class DriverDto
    {
        public ObjectId Id { get; set; }
        public List<ObjectId> PreferredPackageIds { get; set; }
    }
}
