using System.ComponentModel.DataAnnotations;

namespace Rideshare.Application.Features.Auth.Dtos;

public class VerifyOtpRequest
{
  [Required]
  public string PhoneNumber { get; set; } = null!;
  [Required]
  public string OtpCode { get; set; } = null!;
}
