namespace Rideshare.Application.Features.Packages.Dtos
{
    public class LocationDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; } = null!;
        public string PlaceId { get; set; } = null!;

        public LocationDto()
        {
        }
        public LocationDto(double latitude, double longitude, string name, string placeId)
        {
            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            PlaceId = placeId;

        }
    }
}