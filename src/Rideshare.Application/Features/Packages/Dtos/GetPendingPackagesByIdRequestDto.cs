using MongoDB.Bson;

namespace Rideshare.Application.Features.Packages.Dtos;

public class GetPendingPackagesByIdRequestDto
{
   public ObjectId? RiderId {set; get;}
}
