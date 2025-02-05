using MongoDB.Bson;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Application.Responses;


namespace Rideshare.Application.Test.Mocks.Persistence;

public static class MockRiderRepository
{
  public static Mock<IRiderRepository> GetMockRiderRepository()
  {
    var riders = new List<Rider>
    {
      new Rider
      {
        Id = ObjectId.Parse("61f1f7b7d1e7b3b3c8b4b1b1"),
        ApplicationUserId = Guid.NewGuid(),
        FirstName = "Rider 1",
        LastName = "Rider 1",
        PhoneNumber = "1234567890",
        Email = "rider1@ridershare.com",
        ProfilePicture = "profile1.jpg",
        DeviceToken = "deviceToken1",
        UpdatedAt = DateTime.Now
      },
      new Rider
      {
        Id = ObjectId.Parse("61f1f7b7d1e7b3b3c8b4b1b2"),
        ApplicationUserId = Guid.NewGuid(),
        FirstName = "Rider 2",
        LastName = "Rider 2",
        PhoneNumber = "0911847388",
        Email = "rider2@rideshare.com",
        ProfilePicture = "profile2.jpg",
        UpdatedAt = DateTime.Now
      }
    };

    var mockRiderRepository = new Mock<IRiderRepository>();
    mockRiderRepository.Setup(repo => repo.GetAll()).ReturnsAsync(riders);
    mockRiderRepository.Setup(repo => repo.Get(It.IsAny<ObjectId>()))
      .ReturnsAsync((ObjectId id) => riders.FirstOrDefault(r => r.Id == id));
    mockRiderRepository.Setup(repo=>repo.Add(It.IsAny<Rider>() )).ReturnsAsync((Rider ridr)=>
    {
        riders.Add(ridr);
        return ridr;
    });
    mockRiderRepository.Setup(repo => repo.UpdateRiderProfile(It.IsAny<UpdateRiderDto>(), It.IsAny<ObjectId>()))
      .ReturnsAsync((UpdateRiderDto dto, ObjectId id) =>
      {
          var existingRider = riders.FirstOrDefault(r => r.Id == id);

          if (existingRider == null)

          {
              return BaseCommandResponse<Rider>.Failure(new List<string> {"Rider not found" }, "Rider not found");
          }

          existingRider.FirstName = !string.IsNullOrEmpty(dto.FirstName) ? dto.FirstName : existingRider.FirstName;
          existingRider.LastName = !string.IsNullOrEmpty(dto.LastName) ? dto.LastName : existingRider.LastName;
          existingRider.Email = !string.IsNullOrEmpty(dto.Email) ? dto.Email : existingRider.Email;
          existingRider.PhoneNumber = !string.IsNullOrEmpty(dto.PhoneNumber) ? dto.PhoneNumber : existingRider.PhoneNumber;
          existingRider.UpdatedAt = DateTime.Now;

          return BaseCommandResponse<Rider>.Success(existingRider, "updated successfully");
      });

      
      mockRiderRepository.Setup(repo => repo.UpdateRiderProfilePicture(It.IsAny<string>(), It.IsAny<ObjectId>()))
      .ReturnsAsync((string url, ObjectId id) =>
      {
          var existingRider = riders.FirstOrDefault(r => r.Id == id);
          if (existingRider == null)
          {
              return BaseCommandResponse<Rider>.Failure(new List<string> { "Rider not found" }, "Rider not found");
          }

          existingRider.ProfilePicture = url;
          existingRider.UpdatedAt = DateTime.Now;

          return BaseCommandResponse<Rider>.Success(existingRider, "updated successfully");
      });


    return mockRiderRepository;

  }
}