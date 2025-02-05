using MongoDB.Bson;

namespace Rideshare.Application.Features.DriverHistory.Dtos
{
    public class GetDriverHIstoryAndEarningsRequestDto
    {
        public required string DriverId { set; get; }
        public int PageNumber { set; get; }

    }

}