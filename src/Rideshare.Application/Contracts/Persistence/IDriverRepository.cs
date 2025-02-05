using MongoDB.Bson;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence
{
  public interface IDriverRepository : IGenericRepository<Driver>
  {
    Task<List<Package>> GetPreferredPackages(ObjectId driverId);
    Task<Driver> GetByApplicationUserId(Guid applicationUserId);
    Task<Driver> GetDriverById(ObjectId driverId);
    Task<bool> IsEmailUnique(string email);
    Task<bool> IsUsernameUnique(string username);
    Task<bool> IsPhoneNumberUnique(string phoneNumber);
    Task<bool> IsLicenseNumberUnique(string licenseNumber);

    Task<bool> IsLicensePlateNumberUnique(string licensePlateNumber);
    Task<int> Count();
    Task<IReadOnlyList<Driver>> GetDrivers(int skip, int limit);
    Task<IReadOnlyList<Driver>> SearchDrivers(string? SearchTerm, int skip, int limit);

    Task<bool> VerifyDriver(ObjectId driverId);
    Task<bool> DeleteDriver(ObjectId driverId);
    Task<IReadOnlyList<Package>> GetDriverPackages(ObjectId driverId, int skip, int limit);
    Task<bool> ExistsByPhoneNumber(string phoneNumber);
  }
}
