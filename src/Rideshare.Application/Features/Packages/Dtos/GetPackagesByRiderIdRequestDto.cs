using MongoDB.Bson;

namespace Rideshare.Application.Features.Packages.Dtos;

public class GetPackagesByRiderIdRequestDto
{
   public ObjectId RiderId {set; get;}
}
