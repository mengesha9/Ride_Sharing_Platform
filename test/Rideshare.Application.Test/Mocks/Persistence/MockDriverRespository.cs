using MongoDB.Bson;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Test.Mocks.Persistence;

public static class MockDriverRepository
{
  public static Mock<IDriverRepository> GetMockDriverRepository()
  {
    var driver1Id = ObjectId.GenerateNewId();
    var driver2Id = ObjectId.GenerateNewId();
    var driver3Id = ObjectId.GenerateNewId();

    var drivers = new List<Driver>
    {
      new Driver
      {
        Id = driver1Id,
        ApplicationUserId = new Guid(),
        FullName = "Driver 1",
        Password = "password",
        FirstName = "Driver",
        LastName = "One",
        PhoneNumber = "12345678901",
        Email = "driver1@drivers.com",
        Username = "driver1",
        ProfilePicture = "profile.jpg",
        LicenseNumber = "1234567890",
        LicenseExpirationDate = DateTime.Now,
        LicensePlateNumber = "ABC123",
        DriversLicenseImage = "license.jpg",
        CarPhotoFront = "front.jpg",
        CarPhotoBack = "back.jpg",
        CarPhotoLeft = "left.jpg",
        CarPhotoRight = "right.jpg",
        Code3Classification = "Code 3",
        TradeLiscenseInformation = "Trade License",
        VehicleType = VehicleType.Economy,
        IsVerified = false,
        PreferredPackageIds = new List<ObjectId>()
      },
      new Driver
      {
        Id = driver2Id,
        ApplicationUserId = new Guid(),
        FullName = "Driver 2",
        Password = "password",
        FirstName = "Driver",
        LastName = "Two",
        PhoneNumber = "12345678902",
        Email = "driver2@drivers.com",
        Username = "driver2",
        ProfilePicture = "profile.jpg",
        LicenseNumber = "1234567891",
        LicenseExpirationDate = DateTime.Now,
        LicensePlateNumber = "ABC321",
        DriversLicenseImage = "license.jpg",
        CarPhotoFront = "front.jpg",
        CarPhotoBack = "back.jpg",
        CarPhotoLeft = "left.jpg",
        CarPhotoRight = "right.jpg",
        Code3Classification = "Code 3",
        TradeLiscenseInformation = "Trade License",
        VehicleType = VehicleType.Economy,
        IsVerified = false,
        PreferredPackageIds = new List<ObjectId>()
      },
      new Driver
      {
        Id = driver3Id,
        ApplicationUserId = new Guid(),
        FullName = "Driver 3",
        Password = "password",
        FirstName = "Driver",
        LastName = "Three",
        PhoneNumber = "12345678903",
        Email = "driver3@drivers.com",
        Username = "driver3",
        ProfilePicture = "profile.jpg",
        LicenseNumber = "1234567891",
        LicenseExpirationDate = DateTime.Now,
        LicensePlateNumber = "ABC3214",
        DriversLicenseImage = "license.jpg",
        CarPhotoFront = "front.jpg",
        CarPhotoBack = "back.jpg",
        CarPhotoLeft = "left.jpg",
        CarPhotoRight = "right.jpg",
        Code3Classification = "Code 3",
        TradeLiscenseInformation = "Trade License",
        VehicleType = VehicleType.Economy,
        IsVerified = false,
        PreferredPackageIds = new List<ObjectId>()
      }
    };

    var packages = new List<Package> {
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
        AssignedDriver = driver1Id,
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

    var mockDriverRepository = new Mock<IDriverRepository>();
    mockDriverRepository.Setup(repo => repo.GetAll()).ReturnsAsync(drivers);
    mockDriverRepository.Setup(repo => repo.GetDrivers(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((int skip, int limit) =>
    {
      return drivers.Skip(skip - 1).Take(limit).ToList();
    });

    mockDriverRepository.Setup(repo => repo.Get(It.IsAny<ObjectId>()))
      .ReturnsAsync((ObjectId id) => drivers.FirstOrDefault(p => p.Id == id));

    mockDriverRepository.Setup(repo => repo.Add(It.IsAny<Driver>()))
      .ReturnsAsync((Driver package) =>
      {
        drivers.Add(package);
        return package;
      });

    mockDriverRepository.Setup(repo => repo.Exists(It.IsAny<ObjectId>()))
      .ReturnsAsync((ObjectId id) => drivers.Any(p => p.Id == id));

    mockDriverRepository.Setup(repo => repo.Update(It.IsAny<Driver>()))
      .Callback((Driver package) =>
      {
        var index = drivers.FindIndex(p => p.Id == package.Id);
        if (index >= 0)
        {
          drivers[index] = package;
        }
      });

    mockDriverRepository.Setup(repo => repo.Delete(It.IsAny<Driver>()))
      .Callback((Driver package) =>
      {
        var index = drivers.FindIndex(p => p.Id == package.Id);
        if (index >= 0)
        {
          drivers.RemoveAt(index);
        }
      });

    mockDriverRepository.Setup(repo => repo.DeleteDriver(It.IsAny<ObjectId>()))
          .ReturnsAsync((ObjectId id) =>
          {
            var index = drivers.FindIndex(p => p.Id == id);
            if (index >= 0)
            {
              drivers.RemoveAt(index);
              return true;
            }

            return false;
          });

    mockDriverRepository.Setup(repo => repo.VerifyDriver(It.IsAny<ObjectId>()))
      .ReturnsAsync((ObjectId driverId) =>
      {
        var driver = drivers.FirstOrDefault(p => p.Id == driverId);
        if (driver is null)
        {
          return false;
        }


        driver.IsVerified = true;
        return true;
      });

    mockDriverRepository.Setup(repo => repo.IsLicenseNumberUnique(It.IsAny<string>()))
      .ReturnsAsync((string licenseNumber) => !drivers.Any(p => p.LicenseNumber == licenseNumber));

    mockDriverRepository.Setup(repo => repo.GetDriverPackages(It.IsAny<ObjectId>(), It.IsAny<int>(), It.IsAny<int>()))
      .ReturnsAsync((ObjectId driverId, int skip, int limit) => packages.Where(p => p.AssignedDriver == driverId).Skip(skip - 1).Take(limit).ToList());

    mockDriverRepository.Setup(repo => repo.SearchDrivers(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
      .ReturnsAsync((string searchTerm, int skip, int limit) => drivers.Where(d => d.FullName.Contains(searchTerm) || d.FirstName.Contains(searchTerm) || d.LastName.Contains(searchTerm) || d.Email.Contains(searchTerm) || d.PhoneNumber.Contains(searchTerm) || d.LicensePlateNumber.Contains(searchTerm)).Skip(skip - 1).Take(limit).ToList());


    return mockDriverRepository;
  }
}
