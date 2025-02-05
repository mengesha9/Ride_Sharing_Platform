using MongoDB.Bson;
using MongoDB.Driver;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.Infrastructure.Persistence.Repositories;

public class EmailNotificationRepository : GenericRepository<EmailNotification>, IEmailNotificationRepository
{
    private readonly IMongoCollection<EmailNotification> _emailNotificationCollection;

    public EmailNotificationRepository(IMongoDatabase database) : base(database)
    {
        _emailNotificationCollection = database.GetCollection<EmailNotification>("EmailNotification");
    }

    public new async Task<EmailNotification> Get(ObjectId riderId)
    {
        var filter = Builders<EmailNotification>.Filter.Eq("RiderId", riderId);
        return await _emailNotificationCollection.Find(filter).FirstOrDefaultAsync();
    }

}
