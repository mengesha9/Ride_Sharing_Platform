using MongoDB.Bson;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence;

public interface IDriverHistoryRepository : IGenericRepository<DriverHistory>
{
    Task<IReadOnlyList<DriverHistory>> GetByDriverId(ObjectId driverId, int skip, int limit);
}