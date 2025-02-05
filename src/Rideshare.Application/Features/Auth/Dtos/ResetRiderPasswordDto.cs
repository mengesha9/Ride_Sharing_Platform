namespace Rideshare.Application.Features.Auth.Dtos
{
    public class ResetRiderPasswordDto
    {
        public required string PhoneNumber { get; set; }
        public string NewPassword { get; set; } = null!;
    }
}