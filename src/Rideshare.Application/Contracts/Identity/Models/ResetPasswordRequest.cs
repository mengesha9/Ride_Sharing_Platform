namespace Rideshare.Application.Contracts.Identity.Models;

public class ResetPasswordRequest
{
  public string? Username { get; set; }
  public string? Email { get; set; }
  public string? PhoneNumber { get; set; }
}
