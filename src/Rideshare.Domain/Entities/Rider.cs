using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class Rider : BaseEntity
{
  public Guid ApplicationUserId { get; set; }

  public string FirstName { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public string PhoneNumber { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string ProfilePicture { set; get; } = null!;
  public string DeviceToken { set; get; } = null!;
  public DateTime UpdatedAt { set; get; }
}
