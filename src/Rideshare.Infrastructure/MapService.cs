using Microsoft.AspNetCore.WebUtilities;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Domain.Common;

namespace Rideshare.Infrastructure;

public class MapService : IMapService
{
    private readonly HttpClient _httpClient;

    public MapService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<List<string>> FindNearestRoads(List<Location> locations)
    {
        var nearestRoads = new List<string>();
        try
        {
            var baseUri = new Uri("https://roads.googleapis.com/v1/nearestRoads");
            var queryParams = new Dictionary<string, string>
            {
                { "points", GeneratePointsString(locations) },
                { "key", Environment.GetEnvironmentVariable("GoogleMapsApiKey") }
            };

            var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(baseUri.ToString(), queryParams));

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return nearestRoads;
    }

        private string GeneratePointsString(List<Location> locations)
        {
            var points = new List<string>();
            foreach (var location in locations)
            {
                points.Add($"{location.Latitude},{location.Longitude}");
            }
            return string.Join("|", points);
        }
}
