namespace Rideshare.Application.Features.Packages.Strategies.Sorting;

using Rideshare.Domain.Entities;

public class PackageSortingByTotalSeatsStrategy : IPackageSortingStrategy
{
  public List<Package> Sort(List<Package> packages, IPackageSortingOptions filterSettings)
  {
    if (filterSettings.SortBy != PackageSortingCriteria.TotalSeats)
    {
      return packages;
    }

    if (filterSettings.SortValue == PackageSortingValues.Asc)
    {
      return (from package in packages
              orderby package.TotalSeats
              select package).ToList();
    }
    else
    {
      return (from package in packages
              orderby package.TotalSeats descending
              select package).ToList();
    }
  }
}
