using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Drivers.DTOs
{
    public class UpdateDriverInformationResponseDto
    {
        // PERSONAL INFORMATION
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? ProfilePicture { get; set; }

        // DRIVER LICENSE INFORMATION
        public string? LicenseNumber { get; set; }
        public DateTime LicenseExpirationDate { get; set; }
        public string? LicensePlateNumber { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string? DriversLicenseImage { get; set; }
        public string? CarPhotoFront { get; set; }
        public string? CarPhotoBack { get; set; }
        public string? CarPhotoLeft { get; set; }
        public string? CarPhotoRight { get; set; }
        public string? Code3Classification { get; set; }
        public string? TradeLiscenseInformation { get; set; }
        public VehicleType VehicleType { get; set; }
        public bool IsVerified { get; set; } = false;
        public List<ObjectId> PreferredPackageIds { get; set; } = new List<ObjectId>();

    }
}