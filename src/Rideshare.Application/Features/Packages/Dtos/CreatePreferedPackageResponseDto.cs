using MongoDB.Bson;

namespace Rideshare.Application.Features.Packages.Dtos;

public class CreatePreferedPackageResponseDto 
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
}