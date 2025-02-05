using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos
{
    public class GetRiderNotifcationsListRequestDto
    {
        public ObjectId Id {set;get;}
    }
}