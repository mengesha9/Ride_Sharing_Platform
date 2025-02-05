using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Services;

public class DistanceCalculatorService
{
    public static double CalculateDistance(Location location1, Location location2)
    {
        double R = 6371; // Earth radius in kilometers

        double lat1 = ToRadians(location1.Latitude);
        double lon1 = ToRadians(location1.Longitude);
        double lat2 = ToRadians(location2.Latitude);
        double lon2 = ToRadians(location2.Longitude);

        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distance = R * c; // Distance in kilometers
        return distance;
    }
    public static double CalculateDistanceInRadians(Location location1, Location location2)
    {
        double R = 6371; // Earth radius in kilometers

        double dLat = location2.Latitude - location1.Latitude;
        double dLon = location2.Longitude - location1.Longitude;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(location1.Latitude) * Math.Cos(location2.Latitude) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distance = R * c; // Distance in kilometers
        return distance;
    }

    // Function to convert degrees to radians
    private static double ToRadians(double angle)
    {
        return Math.PI * angle / 180.0;
    }
}
