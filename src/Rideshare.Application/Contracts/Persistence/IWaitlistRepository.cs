using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence
{
    public interface IWaitlistRepository : IGenericRepository<Waitlist>
    {
        public Task<(bool EmailExists, bool PhoneNumberExists)> Exists(string email, string phoneNumber);
    }
}