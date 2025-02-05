using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.RiderLocations.Dtos;

public class GetRiderLocationsResponseDto
{
    public ObjectId Id {set; get;}
    public string Name {set; get;}
    public Location Location {set; get;}
}
