using Rideshare.Application.Features.Packages.Strategies.Filtering;
using Rideshare.Application.Features.Packages.Strategies.Pagination;
using Rideshare.Application.Features.Packages.Strategies.Sorting;

namespace Rideshare.Application.Features.Packages.Dtos;


public class GetPackagesDto : IPackageSortingOptions, IPackageFilteringOptions, IPackagePaginationOptions
{
  public int Page { get; set; } = 1;
  public int PageSize { get; set; } = 10;
  public PackageSortingCriteria SortBy { get; set; }
  public PackageSortingValues SortValue { get; set; }
  public PackageFilteringCriteria FilterBy { get; set; }
  public PackageFilteringValues FilterValue { get; set; }
}
