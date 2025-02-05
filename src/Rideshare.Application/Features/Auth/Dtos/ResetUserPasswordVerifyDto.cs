namespace Rideshare.Application.Features.Auth.Dtos;

public class ResetUserPasswordVerifyDto
{
  public string? PhoneNumber { get; set; }
  public string? Username { get; set; }
  public string? Email { get; set; }

  public string Token { get; set; } = null!;
  public string NewPassword { get; set; } = null!;
}
