using MongoDB.Bson;
using MongoDB.Driver;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Infrastructure.Persistence.Repositories;

    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly IMongoCollection<Payment> _paymentCollection;
        private readonly IMongoCollection<Package> _packageCollection;
        private readonly IMongoCollection<User> _userCollection;

        public PaymentRepository(IMongoDatabase database) : base(database)
        {
            _paymentCollection = database.GetCollection<Payment>("Payment");
            _packageCollection = database.GetCollection<Package>("Package");
            _userCollection = database.GetCollection<User>("User");
        }

        public async Task<Payment> AddPaymentAsync(Guid userId, ObjectId packageId, decimal amount)
        {
            var userFilter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var user = await _userCollection.Find(userFilter).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var packageFilter = Builders<Package>.Filter.Eq(p => p.Id, packageId);
            var package = await _packageCollection.Find(packageFilter).FirstOrDefaultAsync();
            if (package == null)
            {
                throw new Exception("Package not found");
            }

            var payment = new Payment
            {
                Id = ObjectId.GenerateNewId(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                PackageId = packageId,
                Amount = package.Price,
                IsSuccessPayment = false,
            };

            await _paymentCollection.InsertOneAsync(payment);

            return payment;
        }

        // public async Task<Payment> ValidatePaymentAsync(ObjectId paymentId)
        // {
        //     var filter = Builders<Payment>.Filter.Eq(p => p.Id, paymentId);
        //     var update = Builders<Payment>.Update.Set(p => p.IsValid, true);
        //     var options = new FindOneAndUpdateOptions<Payment>
        //     {
        //         ReturnDocument = ReturnDocument.After
        //     };

        //     var payment = await _paymentCollection.FindOneAndUpdateAsync(filter, update, options);

        //     return payment;
        // }
    }

