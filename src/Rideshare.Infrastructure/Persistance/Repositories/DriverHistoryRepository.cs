using MongoDB.Bson;
using MongoDB.Driver;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.Infrastructure.Persistence.Repositories;
public class DriverHistoryRepository : GenericRepository<DriverHistory>, IDriverHistoryRepository
{
    private readonly IMongoCollection<DriverHistory> _driverHistoryCollection;

    public DriverHistoryRepository(IMongoDatabase database) : base(database)
    {
        _driverHistoryCollection = database.GetCollection<DriverHistory>("DriverHistory");
    }

    public async Task<IReadOnlyList<DriverHistory>> GetByDriverId(ObjectId driverId, int skip = 0, int limit = 10)
    {
        var driverHistory = await _driverHistoryCollection
            .Find(q => q.DriverId == driverId)
            .Sort(Builders<DriverHistory>.Sort.Descending("Timestamp"))
            .Skip(skip)
            .Limit(limit)
            .ToListAsync();
        return driverHistory;
    }

}