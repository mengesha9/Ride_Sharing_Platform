namespace Rideshare.Application.Features.Auth.Dtos;

public class LoginUserDto
{
  public string? UserName { get; set; }
  public string? Email { get; set; }
  public string? PhoneNumber { get; set; }
  public string Password { get; set; } = null!;
}

