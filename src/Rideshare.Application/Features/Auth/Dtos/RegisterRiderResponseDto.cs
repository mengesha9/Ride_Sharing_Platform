namespace Rideshare.Application.Features.Auth.Dtos;

public class RegisterRiderResponseDto
{
  public UserDto User { get; set; } = null!;
  public string Token { get; set; } = null!;
  public string RefreshToken { get; set; } = null!;

}

