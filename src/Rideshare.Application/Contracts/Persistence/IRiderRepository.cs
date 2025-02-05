using Rideshare.Application.Responses;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Domain.Entities;
using MongoDB.Bson;


namespace Rideshare.Application.Contracts.Persistence;

public interface IRiderRepository : IGenericRepository<Rider>
{
    public Task<bool> ExistsByPhoneNumber(string phoneNumber);
    public Task<Rider> GetByPhoneNUmber(string phoneNumber);
    public Task<Rider> GetByApplicationUserId(Guid applicationUserId);
    public Task<BaseCommandResponse<Rider>> UpdateRiderProfile(UpdateRiderDto rider, ObjectId riderId);
    public Task<BaseCommandResponse<Rider>> UpdateRiderProfilePicture(string Url, ObjectId riderId);
    public Task<List<Rider>> GetPaginated(int pageNumber, int pageSize);

}
