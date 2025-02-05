namespace Rideshare.Application.Features.Packages.Strategies.Sorting;

using Rideshare.Domain.Entities;

public class PackageSortingByNameStrategy : IPackageSortingStrategy
{
  public List<Package> Sort(List<Package> packages, IPackageSortingOptions filterSettings)
  {
    if (filterSettings.SortBy != PackageSortingCriteria.Name)
    {
      return packages;
    }

    if (filterSettings.SortValue == PackageSortingValues.Asc)
    {
      return (from package in packages
              orderby package.Name
              select package).ToList();
    }
    else
    {
      return (from package in packages
              orderby package.Name descending
              select package).ToList();
    }
  }
}
