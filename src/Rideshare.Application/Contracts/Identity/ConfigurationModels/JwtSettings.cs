namespace Rideshare.Application.Contracts.Identity.ConfigurationModels;

public class JwtSettings
{
  public const string SectionName = "IdentityJwtSettings";
  public string Issuer { get; set; } = "";
  public string Audience { get; set; } = "";
  public string Key { get; set; } = "";
  public int DurationInMinutes { get; set; }
  public int RefreshTokenExpiry { get; set; }
}
