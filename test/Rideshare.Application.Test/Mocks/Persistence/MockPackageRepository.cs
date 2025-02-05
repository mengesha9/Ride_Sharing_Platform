using MongoDB.Bson;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Test.Mocks.Persistence;

public static class MockPackageRepository
{
  public static Mock<IPackageRepository> GetMockPackageRepository()
  {
    var packages = new List<Package>
    {
      new Package
      {
        Id = ObjectId.GenerateNewId(),
        Name = "Package 1",
        Price = 100,
        PackageType = PackageType.Weekly,
        VehicleType = VehicleType.Economy,
        PickUpLocation = new Location(1.0,1.0,"Pickup Address 1","PlaceId 1"),
        DropOffLocation = new Location(1.0,1.0,"Dropoff Address 1","PlaceId 1"),
        StartDateTime = DateTime.Now,
        TotalSeats = 10,
        AvailableSeats = 40,
        IsValid = false,
        AssignedDriver = new ObjectId(),
        RegisteredRiders = new List<Rider>()
      },
      new Package
      {
        Id = ObjectId.GenerateNewId(),
        Name = "Package 2",
        Price = 2000,
        PackageType = PackageType.Monthly,
        VehicleType = VehicleType.Classic,
        PickUpLocation = new Location(1.0,1.0,"Pickup Address 2","PlaceId 2"),
        DropOffLocation = new Location(1.0,1.0,"Dropoff Address 2","PlaceId 2"),
        StartDateTime = DateTime.Now,
        TotalSeats = 4,
        AvailableSeats = 400,
        IsValid = true,
        AssignedDriver = ObjectId.Empty,
        RegisteredRiders = new List<Rider>()
      }
    };

    var mockPackageRepository = new Mock<IPackageRepository>();
    mockPackageRepository.Setup(repo => repo.GetAll()).ReturnsAsync(packages);
    mockPackageRepository.Setup(repo => repo.Get(It.IsAny<ObjectId>()))
      .ReturnsAsync((ObjectId id) => packages.FirstOrDefault(p => p.Id == id));
    mockPackageRepository.Setup(repo => repo.GetPackagesByPreference(It.IsAny<Location>(), It.IsAny<Location>(), It.IsAny<DateTime>(), It.IsAny<VehicleType>(), It.IsAny<PackageType>()))
      .ReturnsAsync(packages);
    mockPackageRepository.Setup(repo => repo.GetUnassignedPackages())
      .ReturnsAsync(packages.Where(p => p.AssignedDriver == ObjectId.Empty).ToList());

    mockPackageRepository.Setup(repo => repo.AddPreferredPackage(It.IsAny<ObjectId>(), It.IsAny<ObjectId>()))
      .ReturnsAsync((ObjectId driverId, ObjectId packageId) =>
      {
        var package = packages.FirstOrDefault(p => p.Id == packageId);
        if (package != null)
        {
          package.AssignedDriver = driverId;
        }
        return package;
      });

    mockPackageRepository.Setup(repo => repo.Add(It.IsAny<Package>()))
      .ReturnsAsync((Package package) =>
      {
        packages.Add(package);
        return package;
      });

    mockPackageRepository.Setup(repo => repo.Exists(It.IsAny<ObjectId>()))
      .ReturnsAsync((ObjectId id) => packages.Any(p => p.Id == id));

    mockPackageRepository.Setup(repo => repo.Update(It.IsAny<Package>()))
      .Callback((Package package) =>
      {
        var index = packages.FindIndex(p => p.Id == package.Id);
        if (index >= 0)
        {
          packages[index] = package;
        }
      });

    mockPackageRepository.Setup(repo => repo.Delete(It.IsAny<Package>()))
      .Callback((Package package) =>
      {
        var index = packages.FindIndex(p => p.Id == package.Id);
        if (index >= 0)
        {
          packages.RemoveAt(index);
        }
      });


    return mockPackageRepository;
  }
}
