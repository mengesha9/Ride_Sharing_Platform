namespace Rideshare.Application.Features.Auth.Dtos;

public class UserDto
{
  public string FirstName { get; set; } = null!;
  public string FullName { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public string? PhoneNumber { get; set; }
  public string? Email { get; set; }
  public string? UserName { get; set; }
}
