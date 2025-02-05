namespace Rideshare.Application.Features.Auth.Dtos;

public class ResetUserPasswordVerifyOTPResponseDto
{
  public string PasswordResetToken { get; set; } = null!;
}
