using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Drivers.DTOs
{
    public class UpdateDriverInformationDto
    {
        // PERSONAL INFORMATION
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public IFormFile? ProfilePicture { get; set; }

        // DRIVER LICENSE INFORMATION
        public string? LicenseNumber { get; set; }
        public DateTime? LicenseExpirationDate { get; set; }
        public string? LicensePlateNumber { get; set; }
        [BsonRepresentation(BsonType.String)]
        public IFormFile? DriversLicenseImage { get; set; }
        public IFormFile? CarPhotoFront { get; set; }
        public IFormFile? CarPhotoBack { get; set; }
        public IFormFile? CarPhotoLeft { get; set; }
        public IFormFile? CarPhotoRight { get; set; }
        public IFormFile? Code3Classification { get; set; }
        public IFormFile? TradeLiscenseInformation { get; set; }
        public VehicleType? VehicleType { get; set; }
    }
}