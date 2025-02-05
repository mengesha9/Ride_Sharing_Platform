using Microsoft.AspNetCore.Http;
using Moq;
using Rideshare.Application.Contracts.Infrastructure;

namespace Rideshare.Application.Test.Mocks.Infrastructure;

public static class MockImageUploadService
{
  public static Mock<IImageUploadService> GetMockImageUploadService()
  {
    var images = new List<string> { "https://someurl.com/image-1.jpg" };
    var mockImageUploadService = new Mock<IImageUploadService>();

    mockImageUploadService.Setup(service => service.UploadImage(It.IsAny<IFormFile>()))
      .ReturnsAsync($"https://someurl.com/image-{images.Count + 1}.jpg");

    mockImageUploadService.Setup(service => service.DeleteImage(It.IsAny<string>()))
      .ReturnsAsync((string imageUrl) =>
      {
        var index = images.FindIndex(image => image == imageUrl);
        if (index >= 0)
        {
          images.RemoveAt(index);
          return true;
        }

        return false;
      });

    return mockImageUploadService;
  }
}
