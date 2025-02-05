using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Identity.Models;

public class RegisterUserResponse
{
  public string? Token { get; set; }
  public User? User { get; set; }
  public string RefreshToken { get; set; } = null!;
}
