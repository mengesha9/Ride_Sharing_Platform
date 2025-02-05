namespace Rideshare.Application.Features.Waitlists.Dtos
{
    public class WaitlistRequestDto
    {
        public required string FullName { set; get; }
        public required string Email { set; get; }
        public required string PhoneNumber { set; get; }
    }
}