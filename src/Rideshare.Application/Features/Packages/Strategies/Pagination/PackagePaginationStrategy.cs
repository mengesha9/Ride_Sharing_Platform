namespace Rideshare.Application.Features.Packages.Strategies.Pagination;

using Rideshare.Domain.Entities;

public class PackagePaginationStrategy : IPackagePaginationStrategy
{
  public List<Package> Paginate(List<Package> packages, IPackagePaginationOptions filterSettings)
  {
    return (from package in packages
            select package).Skip((filterSettings.Page - 1) * filterSettings.PageSize).Take(filterSettings.PageSize).ToList();
  }
}
