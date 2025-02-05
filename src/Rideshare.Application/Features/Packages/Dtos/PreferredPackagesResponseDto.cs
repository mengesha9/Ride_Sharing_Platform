using MongoDB.Bson;

namespace Rideshare.Application.Features.Packages.Dtos;

    public class PreferredPackagesResponseDto
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }