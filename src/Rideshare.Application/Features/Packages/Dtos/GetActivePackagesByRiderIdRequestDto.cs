using MongoDB.Bson;

namespace Rideshare.Application.Features.Packages.Dtos;

public class GetActivePackagesByRiderIdRequestDto
{
  public ObjectId RiderId { set; get; }

}