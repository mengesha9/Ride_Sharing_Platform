using MongoDB.Bson;
using MongoDB.Driver;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Infrastructure.Persistence.Repositories;

public class RiderHistoryRepository : GenericRepository<RiderHistory>, IRiderHistoryRepository
{
    private readonly IMongoCollection<RiderHistory> _riderHistoryCollection;
    private readonly IMongoCollection<Package> _packageCollection;
    public RiderHistoryRepository(IMongoDatabase database) : base(database)
    {
        _riderHistoryCollection = database.GetCollection<RiderHistory>("RiderHistory");
        _packageCollection = database.GetCollection<Package>("Package");
    }

    public async Task<int> Count()
    {
        return _riderHistoryCollection == null ? 0 : (int)await _riderHistoryCollection.CountDocumentsAsync(new BsonDocument());
    }
    public async Task<List<RiderHistoryWithPackage>> GetByRiderId(ObjectId riderId, SortField sortField, bool IsAscending, double minPrice, double maxPrice, DateOnly startDate, DateOnly endDate)
    {
        var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);

        var filter = Builders<RiderHistory>.Filter.Eq(x => x.RiderId, riderId) &
                 Builders<RiderHistory>.Filter.Gte(x => x.Price, minPrice) &
                 Builders<RiderHistory>.Filter.Lte(x => x.Price, maxPrice) &
                Builders<RiderHistory>.Filter.Gte(x => x.StartDateTime, startDateTime) &
                Builders<RiderHistory>.Filter.Lte(x => x.StartDateTime, endDateTime);


        var sort = IsAscending
         ? Builders<RiderHistory>.Sort.Ascending(sortField.ToString())
         : Builders<RiderHistory>.Sort.Descending(sortField.ToString());

        var result = await _riderHistoryCollection.Find(filter).Sort(sort).ToListAsync();
        var response = new List<RiderHistoryWithPackage>();
        foreach (var item in result)
        {
            var tmp = Builders<Package>.Filter.Eq(x => x.Id, item.PackageId);
            var package = await _packageCollection.Find(tmp).FirstOrDefaultAsync();
            var riderHistory = new RiderHistoryWithPackage
            {
                Package = package,
                Id = item.Id,
                RiderId = item.RiderId,
                PackageId = item.PackageId,
                PickUpLocation = item.PickUpLocation,
                DropOffLocation = item.DropOffLocation,
                Distance = item.Distance,
                StartDateTime = item.StartDateTime,
                Price = item.Price,
                PackageType = item.PackageType
            };
            response.Add(riderHistory);
        }
        return response;
    }


    public async Task<IReadOnlyList<RiderHistory>> GetByUserId(ObjectId riderId)
    {
        var filter = Builders<RiderHistory>.Filter.Eq(x => x.RiderId, riderId);
        var result = await _riderHistoryCollection.Find(filter).ToListAsync();
        return result;
    }
}
