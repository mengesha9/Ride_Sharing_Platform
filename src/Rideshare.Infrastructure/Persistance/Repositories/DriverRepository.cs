using MongoDB.Bson;
using MongoDB.Driver;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.Infrastructure.Persistence.Repositories;

  public class DriverRepository : GenericRepository<Driver>, IDriverRepository
  {
    private readonly IMongoCollection<Driver>? _driverCollection;
    private readonly IMongoCollection<Package>? _packageCollection;
    public DriverRepository(IMongoDatabase database) : base(database)
    {
      _driverCollection = database.GetCollection<Driver>("Driver");
      _packageCollection = database.GetCollection<Package>("Package");
    }

    public async Task<Driver> GetByApplicationUserId(Guid applicationUserId)
    {
      return await _driverCollection.Find(r => r.ApplicationUserId == applicationUserId).FirstOrDefaultAsync();
    }

    public async Task<bool> IsEmailUnique(string email)
    {
      if (_driverCollection == null)
      {
        return false;
      }
      var filter = Builders<Driver>.Filter.Eq(d => d.Email, email);
      int count = (int)await _driverCollection.CountDocumentsAsync(filter);
      return count == 0;
    }

    public async Task<bool> IsUsernameUnique(string username)
    {
      if (_driverCollection == null)
      {
        return false;
      }
      var filter = Builders<Driver>.Filter.Eq(d => d.Username, username);
      int count = (int)await _driverCollection.CountDocumentsAsync(filter);
      return count == 0;
    }

    public async Task<bool> IsPhoneNumberUnique(string phoneNumber)
    {
      if (_driverCollection == null)
      {
        return false;
      }
      var filter = Builders<Driver>.Filter.Eq(d => d.PhoneNumber, phoneNumber);
      int count = (int)await _driverCollection.CountDocumentsAsync(filter);
      return count == 0;
    }

    public async Task<bool> IsLicenseNumberUnique(string licenseNumber)
    {
      if (_driverCollection == null)
      {
        return false;
      }
      var filter = Builders<Driver>.Filter.Eq(d => d.LicenseNumber, licenseNumber);
      int count = (int)await _driverCollection.CountDocumentsAsync(filter);
      return count == 0;
    }

    public async Task<bool> IsLicensePlateNumberUnique(string licensePlateNumber)
    {
      if (_driverCollection == null)
      {
        return false;
      }
      var filter = Builders<Driver>.Filter.Eq(d => d.LicensePlateNumber, licensePlateNumber);
      int count = (int)await _driverCollection.CountDocumentsAsync(filter);
      return count == 0;
    }

    public async Task<int> Count()
    {
      return _driverCollection == null ? 0 : (int)await _driverCollection.CountDocumentsAsync(new BsonDocument());
    }

    public async Task<IReadOnlyList<Driver>> GetDrivers(int skip, int limit)
    {
      var drivers = await _driverCollection.Find(_ => true).Sort(Builders<Driver>.Sort.Descending("Timestamp"))
       .Skip(skip - 1)
      .Limit(limit)
      .ToListAsync();

      return drivers;
    }

    public async Task<List<Package>> GetPreferredPackages(ObjectId driverId)
    {
      var driver = await _driverCollection.Find(d => d.Id == driverId).FirstOrDefaultAsync();

      if (driver?.PreferredPackageIds == null || !driver.PreferredPackageIds.Any())
      {
        return new List<Package>();
      }

      var packageIds = driver.PreferredPackageIds;
      var packages = await _packageCollection.Find(p => packageIds.Contains(p.Id)).ToListAsync();
      return packages;
    }
    public async Task<bool> VerifyDriver(ObjectId driverId)
    {
      var filter = Builders<Driver>.Filter.Eq(d => d.Id, driverId);
      var update = Builders<Driver>.Update.Set(d => d.IsVerified, true);
      if (_driverCollection == null)
      {
        return false;
      }
      var res = await _driverCollection.UpdateOneAsync(filter, update);
      return res.ModifiedCount > 0;
    }

    public async Task<bool> DeleteDriver(ObjectId driverId)
    {
      var filter = Builders<Driver>.Filter.Eq(d => d.Id, driverId);
      if (_driverCollection == null)
      {
        return false;
      }
      var res = await _driverCollection.DeleteOneAsync(filter);
      return res.DeletedCount > 0;

    }

    public async Task<IReadOnlyList<Driver>> SearchDrivers(string? SearchTerm, int skip, int limit)
    {
      if (SearchTerm == null)
      {
        return await GetDrivers(skip, limit);
      }

      var filter = Builders<Driver>.Filter.Or(
          Builders<Driver>.Filter.Regex(d => d.Username, new BsonRegularExpression(SearchTerm, "i")),
          Builders<Driver>.Filter.Regex(d => d.FirstName, new BsonRegularExpression(SearchTerm, "i")),
          Builders<Driver>.Filter.Regex(d => d.LastName, new BsonRegularExpression(SearchTerm, "i")),
          Builders<Driver>.Filter.Regex(d => d.Email, new BsonRegularExpression(SearchTerm, "i")),
          Builders<Driver>.Filter.Regex(d => d.PhoneNumber, new BsonRegularExpression(SearchTerm, "i")),
          Builders<Driver>.Filter.Regex(d => d.LicensePlateNumber, new BsonRegularExpression(SearchTerm, "i"))
      );

      var drivers = _driverCollection.Find(filter).Sort(Builders<Driver>.Sort.Descending("Timestamp"))
          .Skip(skip - 1)
          .Limit(limit)
          .ToListAsync();
      return await drivers;
    }

    public async Task<IReadOnlyList<Package>> GetDriverPackages(ObjectId driverId, int skip, int limit)
    {
      var filter = Builders<Package>.Filter.Eq(p => p.AssignedDriver, driverId);
      var packages = await _packageCollection.Find(filter).Sort(Builders<Package>.Sort.Descending("Timestamp"))
          .Skip(skip - 1)
          .Limit(limit)
          .ToListAsync();
      return packages;
    }
    public async Task<bool> ExistsByPhoneNumber(string phoneNumber)
    {
      var result = await _driverCollection.Find(r => r.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
      return result != null;
    }

    public Task<Driver> GetDriverById(ObjectId driverId)
    {
      return _driverCollection.Find(r => r.Id == driverId).FirstOrDefaultAsync();
    }
}
