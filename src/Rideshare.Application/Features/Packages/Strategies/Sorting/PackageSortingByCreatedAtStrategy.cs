namespace Rideshare.Application.Features.Packages.Strategies.Sorting;

using Rideshare.Domain.Entities;

public class PackageSortingByCreatedAtStrategy : IPackageSortingStrategy
{
  public List<Package> Sort(List<Package> packages, IPackageSortingOptions filterSettings)
  {
    if (filterSettings.SortBy != PackageSortingCriteria.createdAt)
    {
      return packages;
    }

    if (filterSettings.SortValue == PackageSortingValues.Asc)
    {
      return (from package in packages
              orderby package.createdAt
              select package).ToList();
    }
    else
    {
      return (from package in packages
              orderby package.createdAt descending
              select package).ToList();
    }
  }
}
