using Rideshare.Application.Features.PackageTypes.CQRS.Handlers;
using Rideshare.Application.Features.PackageTypes.CQRS.Requests;
using Rideshare.Application.Test.Mocks.Persistence;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Test.Features.PackageTypes.CQRS.Handlers;

public class GetPackageTypesRequestHandlerTest
{
  [Fact]
  public async Task GetPackageTypesRequestHandler_Success()
  {
    // Arrange
    var mockEnumRepository = MockEnumRepository<PackageType>.GetMockEnumRepository();
    var handler = new GetPackageTypesRequestHandler(mockEnumRepository.Object);
    var request = new GetPackageTypesRequest();

    // Act
    var response = await handler.Handle(request, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.IsSuccess);
  }
}
