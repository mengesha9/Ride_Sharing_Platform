using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Drivers.DTOs
{
    public class GetDriversResponseDto
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public string? ProfilePicture { get; set; }
        public required string PhoneNumber { get; set; }
        public required string LicenseNumber { get; set; }
        public required DateTime LicenseExpirationDate { get; set; }
        public required string LicensePlateNumber { get; set; }
        [BsonRepresentation(BsonType.String)]
        public required VehicleType VehicleType { get; set; }
        public bool IsVerified { get; set; } = false;
        public List<ObjectId> PreferredPackageIds { get; set; } = new List<ObjectId>();
    }
}