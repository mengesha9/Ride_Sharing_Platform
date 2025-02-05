namespace Rideshare.Application.Features.Auth.Dtos;

public class RegisterAdminResponseDto
{
  public UserDto User { get; set; } = null!;
  public string Token { get; set; } = null!;
}

