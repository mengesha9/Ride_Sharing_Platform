namespace Rideshare.Application.Features.Auth.Dtos;

public class RegisterDriverResponseDto
{
  public UserDto User { get; set; } = null!;
  public string Token { get; set; } = null!;
}

