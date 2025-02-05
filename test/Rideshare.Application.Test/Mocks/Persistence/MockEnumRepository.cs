namespace Rideshare.Application.Test.Mocks.Persistence;

using Moq;
using Rideshare.Application.Contracts.Persistence;

public static class MockEnumRepository<TEnum>
{
  public static Mock<IEnumRepository<TEnum>> GetMockEnumRepository()
  {
    var mockEnumRepository = new Mock<IEnumRepository<TEnum>>();

    mockEnumRepository.Setup(repo => repo.GetAllTypeMappings()).Returns(new Dictionary<TEnum, int>());

    return mockEnumRepository;
  }
}
