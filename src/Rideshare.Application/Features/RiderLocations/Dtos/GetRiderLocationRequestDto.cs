using MongoDB.Bson;

namespace Rideshare.Application.Features.RiderLocations.Dtos;

public class GetRiderLocationsRequestDto
{
    public ObjectId RiderId {set; get;}
}
