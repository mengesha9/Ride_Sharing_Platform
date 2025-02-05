using MongoDB.Bson;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence;

public interface IRiderLocationRepository : IGenericRepository<RiderLocation>
{
     Task<IReadOnlyList<RiderLocation>> GetByRiderId(ObjectId riderId);
}
