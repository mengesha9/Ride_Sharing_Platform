using AutoMapper;
using Rideshare.Application.Features.Drivers.CQRS.Handlers;
using Rideshare.Application.Features.Drivers.CQRS.Queries;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Persistence;

namespace Rideshare.Application.Test.Features.Drivers.CQRS.Handlers.Commands;

public class SearchDriversQueryHandlerTest
{
  [Fact]
  public async Task SearchDriversQueryHandler_Success_SearchByFirstName()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
    var handler = new SearchDriversQueryHandler(mockUnitOfWork.Object.DriverRepository, mockMapper);

    var drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    var numberOfDriversBeforeDeletion = drivers.Count;
    var deleteDriverCommand = new SearchDriversQuery
    {
      searchDriversQueryDto = new SearchDriversQueryDto
      {
        SearchTerm = "Driver",
        Page = 1,
        Size = 10
      }
    };

    // Act
    var response = await handler.Handle(deleteDriverCommand, CancellationToken.None);

    // Assert
    drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.Equal(numberOfDriversBeforeDeletion, drivers.Count);
  }

  [Fact]
  public async Task SearchDriversQueryHandler_Success_SearchByLastName()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
    var handler = new SearchDriversQueryHandler(mockUnitOfWork.Object.DriverRepository, mockMapper);

    var drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    var deleteDriverCommand = new SearchDriversQuery
    {
      searchDriversQueryDto = new SearchDriversQueryDto
      {
        SearchTerm = "One",
        Page = 1,
        Size = 10
      }
    };

    // Act
    var response = await handler.Handle(deleteDriverCommand, CancellationToken.None);

    // Assert
    drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.Equal(1, response.Value.Count);
  }

  [Fact]
  public async Task SearchDriversQueryHandler_Success_SearchByUsername()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
    var handler = new SearchDriversQueryHandler(mockUnitOfWork.Object.DriverRepository, mockMapper);

    var drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    var deleteDriverCommand = new SearchDriversQuery
    {
      searchDriversQueryDto = new SearchDriversQueryDto
      {
        SearchTerm = "driver1",
        Page = 1,
        Size = 10
      }
    };

    // Act
    var response = await handler.Handle(deleteDriverCommand, CancellationToken.None);

    // Assert
    drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.Equal(1, response.Value.Count);
  }

  [Fact]
  public async Task SearchDriversQueryHandler_Success_SearchByEmail()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
    var handler = new SearchDriversQueryHandler(mockUnitOfWork.Object.DriverRepository, mockMapper);

    var drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    var deleteDriverCommand = new SearchDriversQuery
    {
      searchDriversQueryDto = new SearchDriversQueryDto
      {
        SearchTerm = "driver1@drivers.com",
        Page = 1,
        Size = 10
      }
    };

    // Act
    var response = await handler.Handle(deleteDriverCommand, CancellationToken.None);

    // Assert
    drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.Equal(1, response.Value.Count);
  }

  [Fact]
  public async Task SearchDriversQueryHandler_Success_SearchByPhoneNumber()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
    var handler = new SearchDriversQueryHandler(mockUnitOfWork.Object.DriverRepository, mockMapper);

    var drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    var deleteDriverCommand = new SearchDriversQuery
    {
      searchDriversQueryDto = new SearchDriversQueryDto
      {
        SearchTerm = "1234567890",
        Page = 1,
        Size = 10
      }
    };

    // Act
    var response = await handler.Handle(deleteDriverCommand, CancellationToken.None);

    // Assert
    drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.Equal(3, response.Value.Count);
  }

  [Fact]
  public async Task SearchDriversQueryHandler_Success_SearchByLicensePlateNumber()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
    var handler = new SearchDriversQueryHandler(mockUnitOfWork.Object.DriverRepository, mockMapper);

    var drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    var deleteDriverCommand = new SearchDriversQuery
    {
      searchDriversQueryDto = new SearchDriversQueryDto
      {
        SearchTerm = "ABC123",
        Page = 1,
        Size = 10
      }
    };

    // Act
    var response = await handler.Handle(deleteDriverCommand, CancellationToken.None);

    // Assert
    drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.Equal(1, response.Value.Count);
  }
}
