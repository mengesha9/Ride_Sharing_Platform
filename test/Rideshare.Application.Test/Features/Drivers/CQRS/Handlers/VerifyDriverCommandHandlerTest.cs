using AutoMapper;
using MongoDB.Bson;
using Rideshare.Application.Features.Drivers.CQRS.Commands;
using Rideshare.Application.Features.Drivers.CQRS.Handlers;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Persistence;

namespace Rideshare.Application.Test.Features.Drivers.CQRS.Handlers;

public class VerifyDriverCommandHandlerTest
{
  [Fact]
  public async Task VerifyDriverCommandHandler_Success()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });
    var mockDriverRepository = MockDriverRepository.GetMockDriverRepository().Object;
    var handler = new VerifyDriverCommandHandler(mockDriverRepository);

    var drivers = await mockDriverRepository.GetAll();
    var numberOfDriversBeforeVerification = drivers.Count;
    var validDriverId = drivers[0].Id;

    var verifyDriverCommand = new VerifyDriverCommand
    {
      verifyDriverDto = new VerifyDriverDto
      {
        driverId = validDriverId.ToString()
      }
    };

    // Act
    var response = await handler.Handle(verifyDriverCommand, CancellationToken.None);

    // Assert
    var driver = await mockDriverRepository.Get(validDriverId);

    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.True(driver?.IsVerified);
  }

  [Fact]
  public async Task VerifyDriverCommandHandler_Failure_IdDoesnotExist()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });
    var mockDriverRepository = MockDriverRepository.GetMockDriverRepository().Object;
    var handler = new VerifyDriverCommandHandler(mockDriverRepository);

    var drivers = await mockDriverRepository.GetAll();
    var verifyDriverCommand = new VerifyDriverCommand
    {
      verifyDriverDto = new VerifyDriverDto
      {
        driverId = (new ObjectId()).ToString()
      }
    };
    // Act
    var response = await handler.Handle(verifyDriverCommand, CancellationToken.None);

    Assert.NotNull(response);
    Assert.False(response.IsSuccess);
    foreach (var driver in drivers)
    {
      Assert.False(driver.IsVerified);
    }
  }
}
