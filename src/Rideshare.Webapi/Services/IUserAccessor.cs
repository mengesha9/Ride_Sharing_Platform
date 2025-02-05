using MongoDB.Bson;

namespace Rideshare.WebApi.Services;

public interface IUserAccessor
{
    ObjectId GetUserId();
    string GetApplicationUserId();
}
