using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class Otp : BaseEntity
{
  public string PhoneNumber { get; set; } = null!;
  public string OtpCode { get; set; } = null!;
  public DateTime ExpiryTime { get; set; } = DateTime.UtcNow.AddMinutes(30);
}
