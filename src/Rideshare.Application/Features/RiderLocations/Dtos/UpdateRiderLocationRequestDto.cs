using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.RiderLocations.Dtos;

public class UpdateRiderLocationRequestDto
{
    public required string Id { set; get; }
    public required string Name { set; get; }
    public required Location Location { set; get; }
}
