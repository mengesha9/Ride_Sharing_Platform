using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Packages.Strategies.Pagination;

public interface IPackagePaginationOptions
{
  public int Page { get; set; }
  public int PageSize { get; set; }
}

public interface IPackagePaginationStrategy
{
  List<Package> Paginate(List<Package> packages, IPackagePaginationOptions filterSettings);
}
