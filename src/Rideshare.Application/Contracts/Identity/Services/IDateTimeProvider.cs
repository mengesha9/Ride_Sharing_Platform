namespace Rideshare.Application.Contracts.Identity.Services;

public interface IDateTimeProvider
{
  public DateTime UtcNow { get; }
}
