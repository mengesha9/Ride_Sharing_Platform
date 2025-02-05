using MongoDB.Bson;

namespace Rideshare.Application.Features.Auth.Dtos
{
  public class RefreshTokenRequest
  {
    public string RefreshToken { get; set; } = null!;
  }
}