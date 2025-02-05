using FirebaseAdmin.Messaging;
using Rideshare.Application.Contracts.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rideshare.Infrastructure.PushNotifications
{
    public class FirebaseNotificationService : INotificationService
    {
        public async Task<bool> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string> data)
        {
            var message = new Message
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                },
                Token = deviceToken,
                Data = (IReadOnlyDictionary<string, string>)data
            };

            var messaging = FirebaseMessaging.DefaultInstance;
            string response = await messaging.SendAsync(message);
            Console.WriteLine("Successfully sent message: " + response);
            if (string.IsNullOrEmpty(response))
            {
                return false;
            }
            return true;
        }
    }
}
