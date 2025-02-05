using AutoMapper;
using MongoDB.Bson;
using Rideshare.Application.Features.Drivers.CQRS.Commands;
using Rideshare.Application.Features.Drivers.CQRS.Handlers;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Infrastructure;
using Rideshare.Application.Test.Mocks.Persistence;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Test.Features.Drivers.CQRS.Handlers.Commands;

public class UpdateDriverInformationCommandHandlerTest
{
  // [Fact]
  // public async Task UpdateDriverInformationCommandHandler_Success()
  // {
  //   // Arrange
  //   var mapperConfig = new MapperConfiguration(c =>
  //       {
  //         c.AddProfile<MappingProfile>();
  //       });
  //
  //   var mockMapper = mapperConfig.CreateMapper();
  //   var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
  //   var mockImageUploadService = MockImageUploadService.GetMockImageUploadService();
  //   var handler = new UpdateDriverInformationCommandHandler(
  //       mockUnitOfWork.Object.DriverRepository,
  //       mockMapper,
  //       mockImageUploadService.Object
  //   );
  //
  //   var drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
  //   var numberOfDriversBeforeDeletion = drivers.Count;
  //   var validDriverId = drivers[0].Id;
  //   var deleteDriverCommand = new UpdateDriverInformationCommand
  //   {
  //     UpdateDriverInformationDto = new UpdateDriverInformationDto
  //     {
  //       FirstName = "Driver",
  //       LastName = "Two",
  //       PhoneNumber = "1234567890",
  //       Email = "driver2@drivers.com",
  //       Username = "driver2",
  //       LicenseNumber = "1234567891",
  //       LicenseExpirationDate = DateTime.Now,
  //       LicensePlateNumber = "ABC321",
  //       VehicleType = VehicleType.Economy,
  //     }
  //   };
  //
  //   // Act
  //   var response = await handler.Handle(deleteDriverCommand, CancellationToken.None);
  //
  //   // Assert
  //   drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
  //   Assert.NotNull(response);
  //   Assert.True(response.IsSuccess);
  //   Assert.Equal(numberOfDriversBeforeDeletion - 1, drivers.Count);
  // }

  [Fact]
  public async Task UpdateDriverInformationCommandHandler_Failure()
  {

  }
}
