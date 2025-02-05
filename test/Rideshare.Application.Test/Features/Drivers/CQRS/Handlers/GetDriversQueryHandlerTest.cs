using AutoMapper;
using Rideshare.Application.Features.Drivers.CQRS.Handlers;
using Rideshare.Application.Features.Drivers.CQRS.Queries;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Persistence;

namespace Rideshare.Application.Test.Features.Drivers.CQRS.Handlers.Commands;

public class GetDriversQueryHandlerTest
{
  [Fact]
  public async Task GetDriversQueryHandler_Success()
  {
    // Arrange

    var mockMapper = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    }).CreateMapper();

    var mockDriverRepository = MockDriverRepository.GetMockDriverRepository().Object;
    var handler = new GetDriversQueryHandler(mockDriverRepository, mockMapper);

    var query = new GetDriversQuery
    {
      getDriversQueryDto = new GetDriversQueryDto
      {
        pageNumber = 1,
        pageSize = 10
      }
    };

    // Act
    var response = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.True(response.IsSuccess);
    Assert.NotEmpty(response.Value);
  }

  [Fact]
  public async Task GetDriversQueryHandler_Success_PageSize()
  {
    // Arrange

    var mockMapper = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    }).CreateMapper();

    var mockDriverRepository = MockDriverRepository.GetMockDriverRepository().Object;
    var handler = new GetDriversQueryHandler(mockDriverRepository, mockMapper);

    var query = new GetDriversQuery
    {
      getDriversQueryDto = new GetDriversQueryDto
      {
        pageNumber = 1,
        pageSize = 1
      }
    };

    // Act
    var response = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.True(response.IsSuccess);
    Assert.Equal(response.Value.Count, 1);
  }

  // [Fact]
  // public async Task GetDriversQueryHandler_Success_PageNumber()
  // {
  //   // Arrange
  //
  //   var mockMapper = new MapperConfiguration(c =>
  //   {
  //     c.AddProfile<MappingProfile>();
  //   }).CreateMapper();
  //
  //   var mockDriverRepository = MockDriverRepository.GetMockDriverRepository().Object;
  //   var handler = new GetDriversQueryHandler(mockDriverRepository, mockMapper);
  //
  //   var drivers = await mockDriverRepository.GetAll();
  //
  //   var query = new GetDriversQuery
  //   {
  //     getDriversQueryDto = new GetDriversQueryDto
  //     {
  //       pageNumber = 2,
  //       pageSize = 1
  //     }
  //   };
  //
  //   // Act
  //   var response = await handler.Handle(query, CancellationToken.None);
  //
  //   // Assert
  //   Assert.True(response.IsSuccess);
  //   Assert.True(response.Value[0].Email.CompareTo(drivers[1].Email) == 0);
  // }
}
