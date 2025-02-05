using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.RiderLocations.Dtos;

public class CreateRiderLocationRequestDto
{
     public ObjectId RiderId {set; get;}
     public string Name {set; get;}
     public Location Location {set; get;}
}
