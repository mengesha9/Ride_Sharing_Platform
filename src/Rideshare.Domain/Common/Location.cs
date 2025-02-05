namespace Rideshare.Domain.Common
{
    public class Location
    {
        public string Name { get; set; } = null!;
        public string PlaceId { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location()
        {
        }
        public Location(double latitude, double longitude, string name, string placeId)
        {
            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            PlaceId = placeId;
        }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

}
