using MongoDB.Bson;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence;

public interface IRiderHistoryRepository : IGenericRepository<RiderHistory>
{
    Task<IReadOnlyList<RiderHistory>> GetByUserId(ObjectId packageId);
    public Task<int> Count();
    public Task<List<RiderHistoryWithPackage>> GetByRiderId(ObjectId riderId, SortField sortField, bool IsAscending, double minPrice, double maxPrice, DateOnly startDate, DateOnly endDate);

}
