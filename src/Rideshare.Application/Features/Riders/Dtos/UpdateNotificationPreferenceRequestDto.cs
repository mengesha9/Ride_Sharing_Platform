using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos;

public class UpdateNotificationPreferenceRequestDto
{
    public ObjectId RiderId {set; get;}
}
