using MongoDB.Bson;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Packages.Strategies.Filtering;

public class PackageFilteringByDriverAssignmentStatusStrategy : IPackageFilteringStrategy
{
  public List<Package> Filter(List<Package> packages, IPackageFilteringOptions filterSettings)
  {
    if (filterSettings.FilterBy != PackageFilteringCriteria.DriverAssignmentStatus)
    {
      return packages;
    }

    if (filterSettings.FilterValue == PackageFilteringValues.True)
    {
      return (from package in packages
              where package.AssignedDriver != ObjectId.Empty
              select package).ToList();
    }
    else
    {
      return (from package in packages
              where package.AssignedDriver == ObjectId.Empty
              select package).ToList();
    }
  }
}
