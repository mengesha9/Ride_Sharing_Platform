using MongoDB.Driver;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Rideshare.Infrastructure.Persistence.Repositories
{
    public class OtpRepository : GenericRepository<Otp>, IOtpRepository
    {
        private readonly IMongoCollection<Otp> _otpCollection;

        public OtpRepository(IMongoDatabase database) : base(database)
        {
            _otpCollection = database.GetCollection<Otp>("Otp");
            EnsureIndexes().Wait();
        }

        private async Task EnsureIndexes()
        {
            try
            {
                var indexKeysDefinition = Builders<Otp>.IndexKeys.Ascending(o => o.createdAt);
                var indexOptions = new CreateIndexOptions { Name = "createdAt_1", ExpireAfter = TimeSpan.FromMinutes(30) };

                var existingIndexes = await _otpCollection.Indexes.ListAsync();
                var indexes = await existingIndexes.ToListAsync();

                var indexExists = indexes.Any(index =>
                {
                    var indexName = index["name"].AsString;
                    return indexName == indexOptions.Name;
                });

                if (indexExists)
                {
                    var existingIndex = indexes.First(index => index["name"].AsString == indexOptions.Name);
                    var existingExpireAfterSeconds = existingIndex["expireAfterSeconds"].ToInt32();

                    if (existingExpireAfterSeconds != indexOptions.ExpireAfter.Value.TotalSeconds)
                    {
                        // Drop the existing index with different options
                        await _otpCollection.Indexes.DropOneAsync(indexOptions.Name);
                        // Create the new index with the correct options
                        await _otpCollection.Indexes.CreateOneAsync(new CreateIndexModel<Otp>(indexKeysDefinition, indexOptions));
                    }
                }
                else
                {
                    // Create the index if it does not exist
                    await _otpCollection.Indexes.CreateOneAsync(new CreateIndexModel<Otp>(indexKeysDefinition, indexOptions));
                }
            }
            catch (Exception ex)
            {
                // Log the exception
            }
        }

        public async Task<Otp> GetByPhoneNumber(string phoneNumber)
        {
            var filter = Builders<Otp>.Filter.Eq("PhoneNumber", phoneNumber);
            var otp = await _otpCollection.Find(filter).FirstOrDefaultAsync();
            if (otp == null)
            {
                throw new NotFoundException("OTP not found for the provided phone number.");
            }
            return otp;
        }

        public async Task<Otp> Add(Otp entity)
        {
            entity.createdAt = DateTime.UtcNow;

            // Remove all previous OTPs for the same phone number
            var filter = Builders<Otp>.Filter.Eq("PhoneNumber", entity.PhoneNumber);
            await _otpCollection.DeleteManyAsync(filter);

            await _otpCollection.InsertOneAsync(entity);

            return entity;
        }

        public async Task<bool> ValidateOtp(string phoneNumber, string otpCode)
        {
            var filter = Builders<Otp>.Filter.Eq("PhoneNumber", phoneNumber);
            var otp = await _otpCollection.Find(filter).FirstOrDefaultAsync();

            if (otp is null)
            {
                throw new NotFoundException("OTP was not found for this phone number.", "OTP");
            }

            if (otp.OtpCode != otpCode)
            {
                throw new BadRequestException("Invalid OTP code.");
            }

            if (otp.ExpiryTime < DateTime.UtcNow)
            {
                throw new BadRequestException("OTP has expired.");
            }

            await DeleteOtp(phoneNumber); // Delete OTP after successful verification

            return true;
        }

        public async Task DeleteOtp(string phoneNumber)
        {
            try
            {
                var filter = Builders<Otp>.Filter.Eq("PhoneNumber", phoneNumber);
                var result = await _otpCollection.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                // Log the exception
            }
        }

        public async Task LogIndexesAndExpiryTimes()
        {
            try
            {
                var indexes = _otpCollection.Indexes.List().ToList();
                foreach (var index in indexes)
                {
                    // Log index details
                }

                var otps = await _otpCollection.Find(Builders<Otp>.Filter.Empty).ToListAsync();
                foreach (var otp in otps)
                {
                    // Log OTP details
                }
            }
            catch (Exception ex)
            {
                // Log the exception
            }
        }
    }
}