using Moq;
using MongoDB.Bson;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rideshare.Application.Test.Mocks.Persistence
{
    public static class MockEmailRepository
    {
        public static Mock<IEmailNotificationRepository> GetMockEmailRepository()
        {
            var mockRepo = new Mock<IEmailNotificationRepository>();
            var emailNotifications = new List<EmailNotification>
            {
                new EmailNotification { RiderId = new ObjectId("000000000000000000000001"), IsEnabled = true },
                new EmailNotification { RiderId = new ObjectId("000000000000000000000002"), IsEnabled = false },
            };

            mockRepo.Setup(r => r.Get(It.IsAny<ObjectId>())).ReturnsAsync((ObjectId id) =>
            {
                return emailNotifications.FirstOrDefault(x => x.RiderId == id);
            });

            mockRepo.Setup(r => r.Add(It.IsAny<EmailNotification>())).Callback((EmailNotification emailNotification) =>
            {
                emailNotifications.Add(emailNotification);
            });

            mockRepo.Setup(r => r.Update(It.IsAny<EmailNotification>())).Callback((EmailNotification emailNotification) =>
            {
                var oldNotification = emailNotifications.FirstOrDefault(x => x.RiderId == emailNotification.RiderId);
                if (oldNotification != null)
                {
                    oldNotification.IsEnabled = emailNotification.IsEnabled;
                }
            });

            return mockRepo;
        }
    }
}
