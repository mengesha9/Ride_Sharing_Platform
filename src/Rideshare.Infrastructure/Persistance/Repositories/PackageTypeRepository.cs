using MongoDB.Driver;
using Rideshare.Domain.Entities;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Infrastructure.Persistence.Repositories;
public class PackageTypeRepository: GenericRepository<PackageTyp>, IPackageTypeRepository
{
    private readonly IMongoCollection<PackageTyp> _packageTypeCollection;
    public PackageTypeRepository(IMongoDatabase database):base(database)
    {
        _packageTypeCollection = database.GetCollection<PackageTyp>("PackageTyp");
    }

    public async Task<PackageTyp> Get(int id)
    {
        return await _packageTypeCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task update(PackageTyp packageType)
    {
        await _packageTypeCollection.ReplaceOneAsync(p => p.Id == packageType.Id, packageType);
    }
}