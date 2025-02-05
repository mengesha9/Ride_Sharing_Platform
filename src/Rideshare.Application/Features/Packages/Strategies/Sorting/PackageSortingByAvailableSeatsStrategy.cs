namespace Rideshare.Application.Features.Packages.Strategies.Sorting;

using Rideshare.Domain.Entities;

public class PackageSortingByAvailableSeatsStrategy : IPackageSortingStrategy
{
  public List<Package> Sort(List<Package> packages, IPackageSortingOptions filterSettings)
  {
    if (filterSettings.SortBy != PackageSortingCriteria.AvailableSeats)
    {
      return packages;
    }

    if (filterSettings.SortValue == PackageSortingValues.Asc)
    {
      return (from package in packages
              orderby package.AvailableSeats
              select package).ToList();
    }
    else
    {
      return (from package in packages
              orderby package.AvailableSeats descending
              select package).ToList();
    }
  }
}
