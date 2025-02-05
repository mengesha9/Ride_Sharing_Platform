using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;
using System.Collections.Generic;

namespace Rideshare.Application.Contracts.Persistence
{
    public interface IEnumRepository<TEnum>
    {

        Dictionary<TEnum, int> GetAllTypeMappings();

    }
}
