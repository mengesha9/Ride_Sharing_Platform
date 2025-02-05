using MongoDB.Bson;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;


namespace Rideshare.Application.Contracts.Persistence;
public interface IEmailNotificationRepository : IGenericRepository<EmailNotification>
{
    Task<EmailNotification> Get(ObjectId riderId);

}
