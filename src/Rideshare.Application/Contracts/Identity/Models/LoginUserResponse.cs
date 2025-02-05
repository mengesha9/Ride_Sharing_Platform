using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Identity.Models;

public class LoginUserResponse
{
  public User User { get; set; } = null!;
  public string Token { get; set; } = null!;
  public string RefreshToken { get; set; } = null!;
}
