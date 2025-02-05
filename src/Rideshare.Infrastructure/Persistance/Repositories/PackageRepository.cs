using MongoDB.Bson;
using MongoDB.Driver;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Services;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Infrastructure.Persistence.Repositories;

public class PackageRepository : GenericRepository<Package>, IPackageRepository
{
    private readonly IMongoCollection<Package> _packageCollection;
    private readonly IMongoCollection<Driver> _driverCollection;
    public PackageRepository(IMongoDatabase database) : base(database)
    {
        _packageCollection = database.GetCollection<Package>("Package");
        _driverCollection = database.GetCollection<Driver>("Driver");
    }
    public async Task<IReadOnlyList<Package>> GetPackagesByPreference(Location pickUpLocation, Location dropOffLocation, DateTime startDateTime, VehicleType vehicleType, PackageType packageType)
    {
        var filterBuilder = Builders<Package>.Filter;
        var filter = filterBuilder.Eq(x => x.PickUpLocation.Latitude, pickUpLocation.Latitude) &
                    filterBuilder.Eq(x => x.PickUpLocation.Longitude, pickUpLocation.Longitude) &
                    filterBuilder.Eq(x => x.DropOffLocation.Latitude, dropOffLocation.Latitude) &
                    filterBuilder.Eq(x => x.DropOffLocation.Longitude, dropOffLocation.Longitude) &
                    filterBuilder.Gte(x => x.StartDateTime, startDateTime.AddMilliseconds(-1)) &
                    filterBuilder.Eq(x => x.VehicleType, vehicleType) &
                    filterBuilder.Eq(x => x.PackageType, packageType);

        var packages = await _packageCollection.Find(filter).ToListAsync();
        return packages;
    }
    // Get packages that are not assigned
    public async Task<IReadOnlyList<Package>> GetUnassignedPackages()
    {
        var filterBuilder = Builders<Package>.Filter;
        var filter = filterBuilder.Eq(x => x.AssignedDriver, ObjectId.Empty);
        var packages = await _packageCollection.Find(filter).ToListAsync();
        return packages;
    }

    public async Task<Package> AddPreferredPackage(ObjectId driverId, ObjectId packageId)
    {
        var filter = Builders<Driver>.Filter.Eq(d => d.Id, driverId);
        var driver = await _driverCollection.Find(filter).FirstOrDefaultAsync();
        if (driver == null)
        {
            throw new Exception("Driver not found");
        }

        var unassignedPackages = await GetUnassignedPackages();
        var preferredPackage = unassignedPackages.FirstOrDefault(p => p.Id == packageId);
        if (preferredPackage != null)
        {
            preferredPackage.AssignedDriver = driverId;
        }
        var replaceFilter = Builders<Package>.Filter.Eq(p => p.Id, preferredPackage.Id);
        await _packageCollection.ReplaceOneAsync(replaceFilter, preferredPackage);

        return preferredPackage;

    }

}
