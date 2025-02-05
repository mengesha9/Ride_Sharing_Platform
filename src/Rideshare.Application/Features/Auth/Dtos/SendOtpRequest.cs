using System.ComponentModel.DataAnnotations;

namespace Rideshare.Application.Features.Auth.Dtos;

public class SendOtpRequest
{
    [Required]
    public string PhoneNumber {get; set;}
}
