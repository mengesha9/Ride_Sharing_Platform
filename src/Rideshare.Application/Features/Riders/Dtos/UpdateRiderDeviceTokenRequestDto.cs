using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos;

public class UpdateRiderDeviceTokenRequestDto
{
    public ObjectId RiderId {set; get;}
    public string DeviceToken {set;get;}
}
