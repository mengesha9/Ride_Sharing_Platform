using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class Admin : BaseEntity
{
  public Guid ApplicationUserId { get; set; }
  public string Email { get; set; } = "";
  public string FirstName { get; set; } = "";
  public string LastName { get; set; } = "";
}
