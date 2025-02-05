using MongoDB.Bson;
using MongoDB.Driver;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Infrastructure.Persistence.Repositories;

    public class RiderNotificationRepository : GenericRepository<RiderNotification>, IRiderNotificationRepository
    {
        private readonly IMongoCollection<RiderNotification>? _riderNotificationCollection;
        public RiderNotificationRepository(IMongoDatabase database) : base(database)
        {
            _riderNotificationCollection = database.GetCollection<RiderNotification>("RiderNotification");
        }

        public async Task<int> Count()
        {
            return _riderNotificationCollection == null ? 0 : (int)await _riderNotificationCollection.CountDocumentsAsync(new BsonDocument());
        }
        public async Task AddAsync(RiderNotification notification)
        {
            await _riderNotificationCollection.InsertOneAsync(notification);
        }

        public async Task SendFullSeatNotification(List<ObjectId> userId, Package choosenPackage)
        {
            var notification = new RiderNotification
            {
                NotificationType = RiderNotificationType.NoSeatAvailable,
                Id = ObjectId.GenerateNewId(),
                Title = "Package has enough riders",
                Description = $"The package {choosenPackage.Name} has enough riders to continue.",
                RiderIds = choosenPackage.RegisteredRiders.Select(r => r.Id).ToList(),
                packageId = choosenPackage.Id
            };
            await AddAsync(notification);
        }


        public async Task<IReadOnlyList<RiderNotification>> GetRiderNotifications(ObjectId riderId)
        {
            return await _riderNotificationCollection.Find(r => r.RiderIds.Contains(riderId)).ToListAsync();
        }
    }
