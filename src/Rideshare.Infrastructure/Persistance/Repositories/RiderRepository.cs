using MongoDB.Driver;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Domain.Entities;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Persistence;
using MongoDB.Bson;


namespace Rideshare.Infrastructure.Persistence.Repositories;

public class RiderRepository : GenericRepository<Rider>, IRiderRepository
{
  private readonly IMongoCollection<Rider>? _riderCollection;

  public RiderRepository(IMongoDatabase database) : base(database)
  {
    _riderCollection = database.GetCollection<Rider>("Rider");
  }

  public async Task<bool> ExistsByPhoneNumber(string phoneNumber)
  {
    var result = await _riderCollection.Find(r => r.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
    return result != null;
  }

  public async Task<Rider> GetByApplicationUserId(Guid applicationUserId)
  {
    return await _riderCollection.Find(r => r.ApplicationUserId == applicationUserId).FirstOrDefaultAsync();
  }

  public async Task<Rider> GetByPhoneNUmber(string phoneNumber)
  {
    return await _riderCollection.Find(r => r.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
  }

    public Task<List<Rider>> GetPaginated(int pageNumber, int pageSize)
    {
        // Validate parameters
        if (pageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));
        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        // Calculate skip and take values
        int skip = (pageNumber - 1) * pageSize;

        // Fetch paginated data
        return _riderCollection.Find(r => true)
          .SortBy(r => r.Id) // Ensure consistent ordering by a unique field
          .Skip(skip)
          .Limit(pageSize)
          .ToListAsync();
    }

    public async Task<BaseCommandResponse<Rider>> UpdateRiderProfile(UpdateRiderDto rider, ObjectId riderId)
  {
    var existingRider = await _riderCollection.Find(r => r.Id == riderId).FirstOrDefaultAsync();

    if (existingRider == null)
    {
      throw new NotFoundException("Rider", rider.FirstName);
    }

    existingRider.FirstName = !string.IsNullOrEmpty(rider.FirstName) ? rider.FirstName : existingRider.FirstName;
    existingRider.LastName = !string.IsNullOrEmpty(rider.LastName) ? rider.LastName : existingRider.LastName;
    existingRider.Email = !string.IsNullOrEmpty(rider.Email) ? rider.Email : existingRider.Email;


    existingRider.UpdatedAt = DateTime.Now;

    var filter = Builders<Rider>.Filter.Eq(r => r.Id, existingRider.Id);
    var updateResult = await _riderCollection.ReplaceOneAsync(filter, existingRider);

    if (updateResult.ModifiedCount > 0)
    {
      return BaseCommandResponse<Rider>.Success(existingRider, "updated successfully");
    }
    else
    {
      return BaseCommandResponse<Rider>.Failure(new List<string> { "server error" }, "Failed to update Rider");
    }

  }

  // public async Task<IReadOnlyList<Rider>>  GetRegisteredRiders()
  // {
  //   var riders = await _riderCollection.Find(r => true).ToListAsync();
  //   return riders;
  // }

  public async Task<BaseCommandResponse<Rider>> UpdateRiderProfilePicture(string imageUrl, ObjectId Id)
  {

    var existingRider = await _riderCollection.Find(r => r.Id == Id).FirstOrDefaultAsync();

    if (existingRider == null)
    {
      throw new NotFoundException("Rider", existingRider.FirstName);
    }

    var filter = Builders<Rider>.Filter.Eq(r => r.Id, Id);
    var update = Builders<Rider>.Update.Set(r => r.ProfilePicture, imageUrl);
    var result = await _riderCollection.UpdateOneAsync(filter, update);
    if (result.ModifiedCount > 0)
    {
      return BaseCommandResponse<Rider>.Success(existingRider, "updated successfully");
    }
    else
    {
      return BaseCommandResponse<Rider>.Failure(new List<string> { "server error" }, "Failed to update the Rider's profile picture");
    }

  }
}

