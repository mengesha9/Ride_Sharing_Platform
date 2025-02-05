namespace Rideshare.Application.Features.Auth.Dtos
{
    public class RegisterDriverDto
    {
        public string FullName { get; set; } = null!;
        public required string PhoneNumber { get; set; }
        public string Password { get; set; } = null!;
    }
}