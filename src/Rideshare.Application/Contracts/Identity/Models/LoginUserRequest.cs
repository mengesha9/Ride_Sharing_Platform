namespace Rideshare.Application.Contracts.Identity.Models;

public class LoginUserRequest
{
  public string? UserName { get; set; }
  public string? Email { get; set; }
  public string? PhoneNumber { get; set; }
  public string Password { get; set; } = null!;
}
