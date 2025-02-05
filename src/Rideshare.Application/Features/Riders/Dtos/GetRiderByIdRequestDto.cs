using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos;

public class GetRiderByIdRequestDto
{
  public ObjectId RiderId { set; get; }
};