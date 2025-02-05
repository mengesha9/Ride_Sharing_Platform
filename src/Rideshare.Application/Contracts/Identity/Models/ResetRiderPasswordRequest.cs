namespace Rideshare.Application.Contracts.Identity.Models
{
    public class ResetRiderPasswordRequest
    {
        public required string PhoneNumber { get; set; }
        public string NewPassword { get; set; } = null!;
    }
}