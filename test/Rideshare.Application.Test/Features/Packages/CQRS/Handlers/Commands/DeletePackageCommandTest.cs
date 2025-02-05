using AutoMapper;
using MongoDB.Bson;
using Rideshare.Application.Features.Packages.CQRS.Handlers.Commands;
using Rideshare.Application.Features.Packages.CQRS.Requests.Commands;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Persistence;

namespace Rideshare.Application.Test.Features.Packages.CQRS.Handlers.Commands;

public class DeletePackageCommandHandlerTest
{
  [Fact]
  public async Task DeletePackageCommandHandler_Success()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
    var handler = new DeletePackageCommandHandler(mockMapper, mockUnitOfWork.Object);

    var packages = await mockUnitOfWork.Object.PackageRepository.GetAll();
    var numberOfPackagesBeforeDeletion = packages.Count;
    var validPackageId = packages[0].Id;
    var deletePackageCommand = new DeletePackageCommand
    {
      DeletePackageDto = new DeletePackageDto
      {
        PackageId = validPackageId
      }
    };

    // Act
    var response = await handler.Handle(deletePackageCommand, CancellationToken.None);

    // Assert
    packages = await mockUnitOfWork.Object.PackageRepository.GetAll();
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
    Assert.Equal(numberOfPackagesBeforeDeletion - 1, packages.Count);
  }

  [Fact]
  public async Task DeletePackageCommandHandler_Failure()
  {
    // Arrange
    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    var mockMapper = mapperConfig.CreateMapper();
    var mockUnitOfWork = MockUnitOfWork.GetMockUnitOfWork();
    var handler = new DeletePackageCommandHandler(mockMapper, mockUnitOfWork.Object);

    var packages = await mockUnitOfWork.Object.PackageRepository.GetAll();
    var numberOfPackagesBeforeDeletion = packages.Count;
    var deletePackageCommand = new DeletePackageCommand
    {
      DeletePackageDto = new DeletePackageDto
      {
        PackageId = new ObjectId()
      }
    };

    // Act
    var response = await handler.Handle(deletePackageCommand, CancellationToken.None);

    // Assert
    packages = await mockUnitOfWork.Object.PackageRepository.GetAll();
    Assert.NotNull(response);
    Assert.False(response.IsSuccess);
    Assert.Equal(numberOfPackagesBeforeDeletion, packages.Count);
  }
}
