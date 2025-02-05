using MongoDB.Driver;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
  private readonly IMongoDatabase _database;
  private IRiderRepository? _riderRepository;
  private IDriverRepository? _driverRepository;
  private IAdminRepository? _adminRepository;
  private IPackageRepository? _packageRepository;
  private IOtpRepository? _otpRepository;
  private IRiderHistoryRepository? _riderHistoryRepository;
  private IRiderNotificationRepository? _riderNotificationRepository;

  public UnitOfWork(IMongoDatabase database) => _database = database;

  public IRiderRepository RiderRepository
  {
    get => _riderRepository ??= new RiderRepository(_database);
  }

  public IDriverRepository DriverRepository
  {
    get => _driverRepository ??= new DriverRepository(_database);
  }
  public IRiderHistoryRepository RiderHistoryRepository
  {
    get => _riderHistoryRepository ??= new RiderHistoryRepository(_database);
  }
  public IRiderNotificationRepository RiderNotificationRepository
  {
    get => _riderNotificationRepository ??= new RiderNotificationRepository(_database);
  }

  public IAdminRepository AdminRepository
  {
    get => _adminRepository ??= new AdminRepository(_database);
  }

  public IPackageRepository PackageRepository
  {
    get => _packageRepository ??= new PackageRepository(_database);
  }

  public IOtpRepository OtpRepository
  {
    get => _otpRepository ??= new OtpRepository(_database);
  }

  public void Dispose()
  {
    GC.SuppressFinalize(this);
  }
}
