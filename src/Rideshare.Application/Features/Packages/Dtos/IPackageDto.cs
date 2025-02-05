using MongoDB.Bson;

namespace Rideshare.Application.Features.Packages.Dtos;

public interface IPackageDto
{
    ObjectId Id { set; get; }
}
