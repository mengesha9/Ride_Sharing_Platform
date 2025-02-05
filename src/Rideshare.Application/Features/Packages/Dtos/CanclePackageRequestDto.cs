using MongoDB.Bson;

namespace Rideshare.Application.Features.Packages.Dtos;

public class CancelPackageRequestDto
{
    public ObjectId PackageId { set; get; }
    public ObjectId RiderId { set; get; }
}
