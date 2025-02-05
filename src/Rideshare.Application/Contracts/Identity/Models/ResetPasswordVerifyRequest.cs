namespace Rideshare.Application.Contracts.Identity.Models;

public class ResetPasswordVerifyRequest
{
  public string? PhoneNumber { get; set; }
  public string? Username { get; set; }
  public string? Email { get; set; }

  public string Token { get; set; } = null!;
  public string NewPassword { get; set; } = null!;
}
