namespace Rideshare.Application.Features.Auth.Dtos;

public class ResetUserPasswordVerifyOTPDto
{
  public string PhoneNumber { get; set; } = null!;
  public string OTP { get; set; } = null!;
}
