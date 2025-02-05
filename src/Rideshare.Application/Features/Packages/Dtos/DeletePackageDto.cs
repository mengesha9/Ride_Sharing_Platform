using MongoDB.Bson;

namespace Rideshare.Application.Features.Packages.Dtos;

public class DeletePackageDto
{
  public ObjectId PackageId { get; set; }
}
