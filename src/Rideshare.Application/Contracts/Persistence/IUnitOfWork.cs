namespace Rideshare.Application.Contracts.Persistence;
public interface IUnitOfWork : IDisposable
{
  public IPackageRepository PackageRepository { get; }
  public IRiderHistoryRepository RiderHistoryRepository { get; }
  public IRiderNotificationRepository RiderNotificationRepository { get; }
  public IRiderRepository RiderRepository { get; }
  public IDriverRepository DriverRepository { get; }
  public IAdminRepository AdminRepository { get; }
  public IOtpRepository OtpRepository { get; }
}

