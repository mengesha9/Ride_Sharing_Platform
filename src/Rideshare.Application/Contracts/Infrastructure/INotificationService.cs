namespace Rideshare.Application.Contracts.Infrastructure;


public interface INotificationService
{
    Task<bool> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string> data);
}
