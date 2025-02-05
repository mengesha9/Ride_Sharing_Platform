using MongoDB.Bson;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence;

public interface IRiderNotificationRepository : IGenericRepository<RiderNotification>
{
    Task SendFullSeatNotification(List<ObjectId> userId, Package packageId);
    Task<int> Count();
    Task<IReadOnlyList<RiderNotification>> GetRiderNotifications(ObjectId riderId);
}
