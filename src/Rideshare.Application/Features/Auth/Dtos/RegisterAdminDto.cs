namespace Rideshare.Application.Features.Auth.Dtos;

public class RegisterAdminDto
{
  public string? Email { get; set; }
  public string FirstName { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public string? UserName { get; set; }
  public string? PhoneNumber { get; set; }
  public string Password { get; set; } = null!;
}
