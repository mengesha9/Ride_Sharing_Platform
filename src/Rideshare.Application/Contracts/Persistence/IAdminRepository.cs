using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence;

public interface IAdminRepository : IGenericRepository<Admin>
{
  Task<Admin> GetByApplicationUserId(Guid applicationUserId);
}

