namespace Rideshare.Application.Features.Drivers.DTOs
{
    public class GetDriverPackagesQueryDto
    {
        public required string DriverId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}