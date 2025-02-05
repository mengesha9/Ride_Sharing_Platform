namespace Rideshare.Application.Contracts.Identity.ConfigurationModels;

public class IdentityDbSettings
{
  public const string SectionName = "IdentityDbSettings";
  public string Name { get; init; } = "";
  public string ConnectionString { get; init; } = "";
}
