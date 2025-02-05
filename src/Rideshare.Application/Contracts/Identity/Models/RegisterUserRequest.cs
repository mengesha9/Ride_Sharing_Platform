namespace Rideshare.Application.Contracts.Identity.Models;

public class RegisterUserRequest
{
  public string? Email { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? FullName { get; set; }
  public string? UserName { get; set; }
  public string PhoneNumber { get; set; } = "";
  public List<string> Roles { get; set; } = null!;
  public string Password { get; set; } = null!;
}
