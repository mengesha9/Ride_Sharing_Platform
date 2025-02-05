using MongoDB.Driver;

namespace Rideshare.Infrastructure.Persistence;

public class MongoDbContext
{
  private readonly IMongoDatabase _database;

  public MongoDbContext(IMongoDatabase database)
  {
    _database = database ?? throw new ArgumentNullException(nameof(database));
  }

  public IMongoCollection<T> GetCollection<T>(string collectionName)
  {
    return _database.GetCollection<T>(collectionName);
  }
}
