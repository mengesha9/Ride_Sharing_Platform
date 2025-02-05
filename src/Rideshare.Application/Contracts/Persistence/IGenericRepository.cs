using System.Linq.Expressions;
using MongoDB.Bson;

namespace Rideshare.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : class
{
  Task<T?> Get(ObjectId id);
  Task<T?> Get(Expression<Func<T, bool>> filter);
  Task<List<T>> GetAll();
  Task<T> Add(T entity);
  Task<bool> Exists(ObjectId id);
  Task Update(T entity);
  Task Delete(T entity);
  Task<List<T>> FindAll(Expression<Func<T, bool>> filter);
}
