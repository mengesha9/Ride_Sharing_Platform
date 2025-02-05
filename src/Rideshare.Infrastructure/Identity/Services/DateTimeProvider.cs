using Rideshare.Application.Contracts.Identity.Services;

namespace Rideshare.Infrastructure.Identity.Services;

public class DateTimeProvider : IDateTimeProvider
{
  public DateTime UtcNow => DateTime.UtcNow;
}
