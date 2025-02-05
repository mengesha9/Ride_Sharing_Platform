
namespace Rideshare.Application.Features.Packages.Dtos
{
  public class LocationRequestDto
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public LocationRequestDto()
    {
    }
    public LocationRequestDto(double latitude, double longitude)
    {
      Latitude = latitude;
      Longitude = longitude;
    }
  }
}