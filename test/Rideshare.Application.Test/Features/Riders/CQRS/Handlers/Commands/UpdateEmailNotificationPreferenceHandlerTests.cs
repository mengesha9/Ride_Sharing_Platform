using AutoMapper;
using MongoDB.Bson;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.CQRS.Handlers.Commands;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Application.Features.Riders.Requests.Commands;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Persistence;
using Rideshare.Domain.Entities;
using Shouldly;
using Xunit;

namespace Rideshare.Application.Test.Features.Riders.CQRS.Handlers.Commands
{
    public class UpdateEmailNotificationPreferenceHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IEmailNotificationRepository> _mockRepo;

        public UpdateEmailNotificationPreferenceHandlerTests()
        {
            _mockRepo = MockEmailRepository.GetMockEmailRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task UpdateEmailNotificationPreferenceHandler_Should_Update_Existing_Record_Success()
        {
            var id = new ObjectId("000000000000000000000001");

            var handler = new UpdateEmailNotificationPreferenceHandler(_mockRepo.Object);
            var request = new UpdateNotificationPreferenceRequestDto
            {
                RiderId = id
            };
            var command = new UpdateNotificationPreferenceRequest
            {
                UpdateNotificationPreferenceRequestDto = request
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.IsSuccess.ShouldBeTrue();
            result.Message.ShouldBe("Email notification preference has been updated.");

            _mockRepo.Verify(repo => repo.Update(It.Is<EmailNotification>(e => e.RiderId == id && e.IsEnabled == false)), Times.Once);
        }

        [Fact]
        public async Task UpdateEmailNotificationPreferenceHandler_Should_Add_New_Record_If_Not_Exist()
        {
            var id = ObjectId.GenerateNewId();

            var handler = new UpdateEmailNotificationPreferenceHandler(_mockRepo.Object);
            var request = new UpdateNotificationPreferenceRequestDto
            {
                RiderId = id
            };
            var command = new UpdateNotificationPreferenceRequest
            {
                UpdateNotificationPreferenceRequestDto = request
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.IsSuccess.ShouldBeTrue();
            result.Message.ShouldBe("Email notification preference has been updated.");

            _mockRepo.Verify(repo => repo.Add(It.Is<EmailNotification>(e => e.RiderId == id && e.IsEnabled == true)), Times.Once);
        }
    }
}
