using AutoMapper;
using Rideshare.Application.Features.Drivers.CQRS.Handlers;
using Rideshare.Application.Features.Drivers.CQRS.Queries;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Persistence;

namespace Rideshare.Application.Test.Features.Drivers.CQRS.Handlers.Commands;

public class GetDriverPackagesQueryHandlerTest
{
  [Fact]
  public async Task GetDriverPackagesQueryHandler_Success()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
    var handler = new GetDriverPackagesQueryHandler(mockUnitOfWork.Object.DriverRepository, mockMapper);

    var drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    var validDriverId = drivers[0].Id;

    var deleteDriverCommand = new GetDriverPackagesQuery
    {
      getDriverPackagesQueryDto = new GetDriverPackagesQueryDto
      {
        DriverId = validDriverId.ToString(),
        PageNumber = 1,
        PageSize = 10
      }
    };

    // Act
    var response = await handler.Handle(deleteDriverCommand, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.Equal(response.Value.Count, 1);
  }

  [Fact]
  public async Task GetDriverPackagesQueryHandler_Failure()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
    var handler = new GetDriverPackagesQueryHandler(mockUnitOfWork.Object.DriverRepository, mockMapper);

    var drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
    var validDriverId = drivers[1].Id;

    var deleteDriverCommand = new GetDriverPackagesQuery
    {
      getDriverPackagesQueryDto = new GetDriverPackagesQueryDto
      {
        DriverId = validDriverId.ToString(),
        PageNumber = 1,
        PageSize = 10
      }
    };

    // Act
    var response = await handler.Handle(deleteDriverCommand, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.Equal(response.Value.Count, 0);
  }
}
