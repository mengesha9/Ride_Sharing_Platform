using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos;

public class GetRiderProfileRequestDto
{
    public ObjectId Id {set;get;}
}
