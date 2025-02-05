using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Packages.Strategies.Filtering;

public enum PackageFilteringCriteria
{
  None,
  Validity, // Will be used to filter packages based on their validity. Utilizes the IsValid attribute
  DriverAssignmentStatus // Will be used to filter packages based on their driver assignment status. Utilizes the DriverId attribute
}

public enum PackageFilteringValues
{
  True,
  False
}

public interface IPackageFilteringOptions
{
  public PackageFilteringCriteria FilterBy { get; set; }
  public PackageFilteringValues FilterValue { get; set; }
}

public interface IPackageFilteringStrategy
{
  List<Package> Filter(List<Package> packages, IPackageFilteringOptions filterSettings);
}
