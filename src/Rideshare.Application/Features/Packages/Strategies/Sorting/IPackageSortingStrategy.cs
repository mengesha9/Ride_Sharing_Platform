using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Packages.Strategies.Sorting;

public enum PackageSortingCriteria
{
  None,
  createdAt,
  Name,
  Price,
  TotalSeats,
  AvailableSeats,
  StartDateTime
}

public enum PackageSortingValues
{
  Asc,
  Dsc
}

public interface IPackageSortingOptions
{
  public PackageSortingCriteria SortBy { get; set; }
  public PackageSortingValues SortValue { get; set; }
}

public interface IPackageSortingStrategy
{
  List<Package> Sort(List<Package> packages, IPackageSortingOptions filterSettings);
}
