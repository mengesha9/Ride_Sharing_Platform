using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Infrastructure.Persistence.Repositories;

public class EnumRepository<TEnum> : IEnumRepository<TEnum> where TEnum : Enum
{
  private readonly Dictionary<TEnum, int> _enumMappings;

  public EnumRepository(Dictionary<TEnum, int> enumMappings)
  {
    _enumMappings = enumMappings;
  }

  public Dictionary<TEnum, int> GetAllTypeMappings()
  {
    return _enumMappings;
  }
}
