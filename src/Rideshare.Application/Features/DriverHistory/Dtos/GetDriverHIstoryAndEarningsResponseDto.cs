using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.DriverHistory.Dtos
{
    public class GetDriverHIstoryAndEarningsResponseDto
    {
        public ObjectId Id { set; get; }
        public ObjectId DriverId { set; get; }
        public required Location PickUpLocation { set; get; }
        public required Location DropOffLocation { set; get; }
        public required bool PaymentStatus { set; get; }
        public required double TotalEarning { set; get; }
    }

}