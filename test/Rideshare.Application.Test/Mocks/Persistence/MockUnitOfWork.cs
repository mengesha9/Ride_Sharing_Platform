using Moq;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Application.Test.Mocks.Persistence;

public class MockUnitOfWork
{
  public static Mock<IUnitOfWork> GetMockUnitOfWork()
  {
    var unitOfWork = new Mock<IUnitOfWork>();

    unitOfWork.Setup(uow => uow.PackageRepository).Returns(MockPackageRepository.GetMockPackageRepository().Object);
    unitOfWork.Setup(uow => uow.DriverRepository).Returns(MockDriverRepository.GetMockDriverRepository().Object);

    return unitOfWork;
  }
}
