namespace Rideshare.Application.Features.Packages.Dtos
{
    public class GetNearbyPackagesDto
    {
        public LocationRequestDto UserLocation { get; set; } = null!;
        public LocationRequestDto? Destination { get; set; }
    }
}