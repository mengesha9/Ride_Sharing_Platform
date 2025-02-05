using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Rideshare.Domain.Entities;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>, User
{
  override public Guid Id { get; set; }
  public string FirstName { get; set; } = "";
  public string LastName { get; set; } = "";
  public string FullName { get; set; } = "";
  public RefreshTokenModel? RefreshToken { get; set; }
}
