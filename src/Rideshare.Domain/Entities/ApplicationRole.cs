using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Rideshare.Domain.Entities;

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<Guid>
{ }

