using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities
{
    public class Waitlist : BaseEntity
    {
        public string FullName { set; get; } = null!;
        public string Email { set; get; } = null!;
        public string PhoneNumber { set; get; } = null!;
    }
}