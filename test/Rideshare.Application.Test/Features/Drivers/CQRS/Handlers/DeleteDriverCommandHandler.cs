// using AutoMapper;
// using MongoDB.Bson;
// using Rideshare.Application.Features.Drivers.CQRS.Commands;
// using Rideshare.Application.Features.Drivers.CQRS.Handlers;
// using Rideshare.Application.Features.Drivers.DTOs;
// using Rideshare.Application.Profiles;
// using Rideshare.Application.Test.Mocks.Persistence;
// using Rideshare.Application.Contracts.Identity.Services;

// namespace Rideshare.Application.Test.Features.Drivers.CQRS.Handlers.Commands;

// public class DeleteDriverCommandHandlerTest
// {
//   [Fact]
//   public async Task DeleteDriverCommandHandler_Success()
//   {
//     // Arrange
//     var mapperConfig = new MapperConfiguration(c =>
//         {
//           c.AddProfile<MappingProfile>();
//         });

//     var mockMapper = mapperConfig.CreateMapper();
//     var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
//     var handler = new DeleteDriverCommandHandler(mockUnitOfWork.Object.DriverRepository, mockUnitOfWork.Object.Auth);

//     var drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
//     var numberOfDriversBeforeDeletion = drivers.Count;
//     var validDriverId = drivers[0].Id;
//     var deleteDriverCommand = new DeleteDriverCommand
//     {
//       deleteDriverDto = new DeleteDriverDto
//       {
//         driverId = validDriverId.ToString()
//       }
//     };

//     // Act
//     var response = await handler.Handle(deleteDriverCommand, CancellationToken.None);

//     // Assert
//     drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
//     Assert.NotNull(response);
//     Assert.True(response.IsSuccess);
//     Assert.Equal(numberOfDriversBeforeDeletion - 1, drivers.Count);
//   }

//   [Fact]
//   public async Task DeleteDriverCommandHandler_Failure()
//   {
//     // Arrange
//     var mapperConfig = new MapperConfiguration(c =>
//         {
//           c.AddProfile<MappingProfile>();
//         });

//     var mockMapper = mapperConfig.CreateMapper();
//     var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
//     var handler = new DeleteDriverCommandHandler(mockUnitOfWork.Object.DriverRepository);

//     var drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
//     var numberOfDriversBeforeDeletion = drivers.Count;
//     var deleteDriverCommand = new DeleteDriverCommand
//     {
//       deleteDriverDto = new DeleteDriverDto
//       {
//         driverId = (new ObjectId()).ToString()
//       }
//     };

//     // Act
//     var response = await handler.Handle(deleteDriverCommand, CancellationToken.None);

//     // Assert
//     drivers = await mockUnitOfWork.Object.DriverRepository.GetAll();
//     Assert.NotNull(response);
//     Assert.False(response.IsSuccess);
//     Assert.Equal(numberOfDriversBeforeDeletion, drivers.Count);
//   }
// }
