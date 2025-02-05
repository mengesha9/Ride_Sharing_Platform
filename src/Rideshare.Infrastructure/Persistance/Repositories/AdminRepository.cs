using MongoDB.Driver;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.Infrastructure.Persistence.Repositories;

public class AdminRepository : GenericRepository<Admin>, IAdminRepository
{
  private readonly IMongoCollection<Admin>? _adminCollection;

  public AdminRepository(IMongoDatabase database) : base(database)
  {
    _adminCollection = database.GetCollection<Admin>("Admin");
  }

  public async Task<Admin> GetByApplicationUserId(Guid applicationUserId)
  {
    return await _adminCollection.Find(r => r.ApplicationUserId == applicationUserId).FirstOrDefaultAsync();
  }
}
