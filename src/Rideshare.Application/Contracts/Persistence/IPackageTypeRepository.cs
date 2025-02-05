using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence;
public interface IPackageTypeRepository:IGenericRepository<PackageTyp>
{
   Task<PackageTyp> Get(int id);
}