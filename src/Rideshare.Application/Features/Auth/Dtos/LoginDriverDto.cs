namespace Rideshare.Application.Features.Auth.Dtos
{
    public class LoginDriverDto
    {
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}