namespace Rideshare.Domain.Entities;

public interface User
{
  public Guid Id { get; set; }
  public string? Email { get; set; }
  public string FullName { get; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string? UserName { get; set; }
  public string? PhoneNumber { get; set; }
  public List<Guid> Roles { get; set; }
}
