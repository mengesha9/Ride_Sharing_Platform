using MongoDB.Driver;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using Rideshare.Infrastructure.Persistence.Repositories;

namespace Rideshare.Infrastructure.Persistance.Repositories
{
    public class WaitlistRepository : GenericRepository<Waitlist>, IWaitlistRepository
    {
        private readonly IMongoCollection<Waitlist>? _waitlistCollection;
        public WaitlistRepository(IMongoDatabase database) : base(database)
        {
            _waitlistCollection = database.GetCollection<Waitlist>("Waitlist");
        }

        public async Task<(bool EmailExists, bool PhoneNumberExists)> Exists(string email, string phoneNumber)
        {
            var emailExists = await _waitlistCollection.Find(w => w.Email == email).AnyAsync();
            var phoneNumberExists = await _waitlistCollection.Find(w => w.PhoneNumber == phoneNumber).AnyAsync();
            return (emailExists, phoneNumberExists);
        }

    }
}