using AutoMapper;
using MongoDB.Bson;
using Rideshare.Application.Features.Packages.CQRS.Handlers.Queries;
using Rideshare.Application.Features.Packages.CQRS.Requests.Queries;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.Application.Features.Packages.Strategies.Filtering;
using Rideshare.Application.Features.Packages.Strategies.Sorting;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Persistence;

namespace Rideshare.Application.Test.Features.Packages.CQRS.Handlers.Queries;

public class GetPackagesQueryHandlerTest
{
  [Fact]
  public async Task GetPackagesQueryHandler_Success()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();

    var handler = new GetPackagesQueryHandler(mockMapper, mockUnitOfWork.Object);
    var packages = await mockUnitOfWork.Object.PackageRepository.GetAll();

    var getPackagesQuery = new GetPackagesQuery
    {
      GetPackagesDto = new GetPackagesDto
      {
        Page = 1,
        PageSize = 2,
        SortBy = PackageSortingCriteria.None,
        SortValue = PackageSortingValues.Asc,
        FilterBy = PackageFilteringCriteria.None,
        FilterValue = PackageFilteringValues.True
      }
    };

    // Act
    var response = await handler.Handle(getPackagesQuery, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.Equal(packages.Count, response.Value.Count);
  }

  [Fact]
  public async Task GetPackagesQueryHandler_Positive_SortBy_CreatedAt()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();

    var handler = new GetPackagesQueryHandler(mockMapper, mockUnitOfWork.Object);

    var getPackagesQuery = new GetPackagesQuery
    {
      GetPackagesDto = new GetPackagesDto
      {
        Page = 1,
        PageSize = 2,
        SortBy = PackageSortingCriteria.createdAt,
        SortValue = PackageSortingValues.Asc,
        FilterBy = PackageFilteringCriteria.None,
        FilterValue = PackageFilteringValues.True
      }
    };

    // Act
    var response = await handler.Handle(getPackagesQuery, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.True(response.Value[0].createdAt <= response.Value[1].createdAt);
  }

  [Fact]
  public async Task GetPackagesQueryHandler_Positive_SortBy_Name()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();

    var handler = new GetPackagesQueryHandler(mockMapper, mockUnitOfWork.Object);

    var getPackagesQuery = new GetPackagesQuery
    {
      GetPackagesDto = new GetPackagesDto
      {
        Page = 1,
        PageSize = 2,
        SortBy = PackageSortingCriteria.Name,
        SortValue = PackageSortingValues.Asc,
        FilterBy = PackageFilteringCriteria.None,
        FilterValue = PackageFilteringValues.True
      }
    };

    // Act
    var response = await handler.Handle(getPackagesQuery, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.Equal(-1, response.Value[0].Name.CompareTo(response.Value[1].Name));
  }

  [Fact]
  public async Task GetPackagesQueryHandler_Positive_SortBy_Price()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();

    var handler = new GetPackagesQueryHandler(mockMapper, mockUnitOfWork.Object);

    var getPackagesQuery = new GetPackagesQuery
    {
      GetPackagesDto = new GetPackagesDto
      {
        Page = 1,
        PageSize = 2,
        SortBy = PackageSortingCriteria.Price,
        SortValue = PackageSortingValues.Asc,
        FilterBy = PackageFilteringCriteria.None,
        FilterValue = PackageFilteringValues.True
      }
    };

    // Act
    var response = await handler.Handle(getPackagesQuery, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.True(response.Value[0].Price <= response.Value[1].Price);
  }

  [Fact]
  public async Task GetPackagesQueryHandler_Positive_SortBy_TotalSeats()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();

    var handler = new GetPackagesQueryHandler(mockMapper, mockUnitOfWork.Object);

    var getPackagesQuery = new GetPackagesQuery
    {
      GetPackagesDto = new GetPackagesDto
      {
        Page = 1,
        PageSize = 2,
        SortBy = PackageSortingCriteria.TotalSeats,
        SortValue = PackageSortingValues.Asc,
        FilterBy = PackageFilteringCriteria.None,
        FilterValue = PackageFilteringValues.True
      }
    };

    // Act
    var response = await handler.Handle(getPackagesQuery, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.True(response.Value[0].TotalSeats <= response.Value[1].TotalSeats);
  }

  [Fact]
  public async Task GetPackagesQueryHandler_Positive_SortBy_AvailableSeats()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();

    var handler = new GetPackagesQueryHandler(mockMapper, mockUnitOfWork.Object);

    var getPackagesQuery = new GetPackagesQuery
    {
      GetPackagesDto = new GetPackagesDto
      {
        Page = 1,
        PageSize = 2,
        SortBy = PackageSortingCriteria.AvailableSeats,
        SortValue = PackageSortingValues.Asc,
        FilterBy = PackageFilteringCriteria.None,
        FilterValue = PackageFilteringValues.True
      }
    };

    // Act
    var response = await handler.Handle(getPackagesQuery, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.True(response.Value[0].AvailableSeats <= response.Value[1].AvailableSeats);
  }

  [Fact]
  public async Task GetPackagesQueryHandler_Positive_SortBy_StartDateTime()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();

    var handler = new GetPackagesQueryHandler(mockMapper, mockUnitOfWork.Object);

    var getPackagesQuery = new GetPackagesQuery
    {
      GetPackagesDto = new GetPackagesDto
      {
        Page = 1,
        PageSize = 2,
        SortBy = PackageSortingCriteria.StartDateTime,
        SortValue = PackageSortingValues.Asc,
        FilterBy = PackageFilteringCriteria.None,
        FilterValue = PackageFilteringValues.True
      }
    };

    // Act
    var response = await handler.Handle(getPackagesQuery, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.True(response.Value[0].createdAt <= response.Value[1].createdAt);
  }

  [Fact]
  public async Task GetPackagesQueryHandler_Positive_FilterBy_Validity()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();

    var handler = new GetPackagesQueryHandler(mockMapper, mockUnitOfWork.Object);

    var getPackagesQuery = new GetPackagesQuery
    {
      GetPackagesDto = new GetPackagesDto
      {
        Page = 1,
        PageSize = 2,
        SortBy = PackageSortingCriteria.StartDateTime,
        SortValue = PackageSortingValues.Asc,
        FilterBy = PackageFilteringCriteria.Validity,
        FilterValue = PackageFilteringValues.True
      }
    };

    // Act
    var response = await handler.Handle(getPackagesQuery, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);

    foreach (var package in response.Value)
    {
      Assert.True(package.IsValid);
    }
  }

  [Fact]
  public async Task GetPackagesQueryHandler_Positive_FilterBy_DriverAssignmentStatus()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();

    var handler = new GetPackagesQueryHandler(mockMapper, mockUnitOfWork.Object);

    var getPackagesQuery = new GetPackagesQuery
    {
      GetPackagesDto = new GetPackagesDto
      {
        Page = 1,
        PageSize = 2,
        SortBy = PackageSortingCriteria.StartDateTime,
        SortValue = PackageSortingValues.Asc,
        FilterBy = PackageFilteringCriteria.DriverAssignmentStatus,
        FilterValue = PackageFilteringValues.True
      }
    };

    // Act
    var response = await handler.Handle(getPackagesQuery, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);

    foreach (var package in response.Value)
    {
      Assert.NotEqual(package.AssignedDriver, ObjectId.Empty);
    }
  }
}
