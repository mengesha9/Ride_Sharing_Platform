namespace Rideshare.Application.Features.Packages.Strategies.Sorting;

using Rideshare.Domain.Entities;

public class PackageSortingByStartDateTimeStrategy : IPackageSortingStrategy
{
  public List<Package> Sort(List<Package> packages, IPackageSortingOptions filterSettings)
  {
    if (filterSettings.SortBy != PackageSortingCriteria.StartDateTime)
    {
      return packages;
    }

    if (filterSettings.SortValue == PackageSortingValues.Asc)
    {
      return (from package in packages
              orderby package.StartDateTime
              select package).ToList();
    }
    else
    {
      return (from package in packages
              orderby package.StartDateTime descending
              select package).ToList();
    }
  }
}
