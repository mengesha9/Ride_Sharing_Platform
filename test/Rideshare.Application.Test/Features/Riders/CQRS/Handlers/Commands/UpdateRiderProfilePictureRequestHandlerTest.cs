using AutoMapper;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Moq;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Riders.CQRS.Handlers;
using Rideshare.Application.Features.Riders.CQRS.Requests.Commands;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Infrastructure;
using Rideshare.Application.Test.Mocks.Persistence;
using Rideshare.Domain.Entities;
using Shouldly;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Rideshare.Application.Test.Features.Riders.CQRS.Handlers.Commands
{
    public class UpdateRiderProfilePictureRequestHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRiderRepository> _mockRepo;
        private readonly Mock<IImageUploadService> _mockImageUploadService;

        public UpdateRiderProfilePictureRequestHandlerTests()
        {
            _mockRepo = MockRiderRepository.GetMockRiderRepository();
            _mockImageUploadService = MockImageUploadService.GetMockImageUploadService();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task UpdateRiderProfilePictureRequestHandler_Success()
        {
            var id = ObjectId.GenerateNewId();
            var newRider = new Rider
            {
                Id = id,
                FirstName = "",
                LastName = "",
                Email = "",
                ProfilePicture = "",
                PhoneNumber = "251988888888"
            };
            await _mockRepo.Object.Add(newRider);
            var handler = new UpdateRiderProfilePictureRequestHandler(_mockRepo.Object, _mockImageUploadService.Object);
            var imageFilePath = "../../../Assets/testImage.jpg";
            using (var imageStream = File.OpenRead(imageFilePath))
            {
                var imageFile = new FormFile(imageStream, 0, imageStream.Length, null, Path.GetFileName(imageFilePath))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpg"
                };

                var request = new UpdateRiderProfilePictureRequest
                {
                    riderID = id,
                    Image = imageFile
                };
                var result = await handler.Handle(request, CancellationToken.None);
                var rider = await _mockRepo.Object.Get(id);
                Console.WriteLine(result.Errors);
                result.ShouldNotBeNull();
                result.Succeeded.ShouldBeTrue();
                result.Message.ShouldBe("updated successfully");
                rider.ProfilePicture.ShouldNotBeNullOrEmpty();
            }
        }

        [Fact]
        public async Task UpdateRiderProfilePictureRequestHandler_Failure()
        {
            
            var handler = new UpdateRiderProfilePictureRequestHandler(_mockRepo.Object, _mockImageUploadService.Object);
            var imageFilePath = "../../../Assets/testImage.jpg";
            using (var imageStream = File.OpenRead(imageFilePath))
            {
                var id = ObjectId.GenerateNewId();

                var imageFile = new FormFile(imageStream, 0, imageStream.Length, null, Path.GetFileName(imageFilePath))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpg"
                };

                var request = new UpdateRiderProfilePictureRequest
                {
                    riderID = id,
                    Image = imageFile
                };
                var result = await handler.Handle(request, CancellationToken.None);
                result.ShouldNotBeNull();
                result.Succeeded.ShouldBeFalse();
                result.Message.ShouldBe("Rider not found");
            }
        }
        
    }
}