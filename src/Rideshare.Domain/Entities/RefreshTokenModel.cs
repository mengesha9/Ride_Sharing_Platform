namespace Rideshare.Domain.Entities;
public class RefreshTokenModel
{
  public required string Token { get; set; }
  public DateTime ExpiresIn { get; set; }
  public DateTime Created { get; set; } = DateTime.UtcNow;
  public DateTime? Revoked { get; set; }
  public bool IsRevoked => Revoked != null;
}