using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Packages.Strategies.Filtering;

public class PackageFilteringByValidityStrategy : IPackageFilteringStrategy
{
  public List<Package> Filter(List<Package> packages, IPackageFilteringOptions filterSettings)
  {
    if (filterSettings.FilterBy != PackageFilteringCriteria.Validity)
    {
      return packages;
    }

    if (filterSettings.FilterValue == PackageFilteringValues.True)
    {
      return (from package in packages
              where package.IsValid == true
              select package).ToList();
    }
    else
    {
      return (from package in packages
              where !package.IsValid == false
              select package).ToList();
    }
  }
}
