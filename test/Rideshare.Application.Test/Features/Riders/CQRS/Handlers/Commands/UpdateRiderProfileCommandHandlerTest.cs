using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MongoDB.Bson;
using Rideshare.Application.Features.CQRS.Handlers.Commands;
using Rideshare.Application.Features.Riders.Requests.Commands;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Persistence;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using Shouldly;


namespace Rideshare.Application.Test.Features.Riders.CQRS.Handlers.Commands;
public class UpdateRiderProfileRequestHandlerTest

{    
    private readonly IMapper _mapper;
    private readonly Mock<IRiderRepository> _mockRepo;

    public UpdateRiderProfileRequestHandlerTest()
    {
        _mockRepo=MockRiderRepository.GetMockRiderRepository();
        
        var mapperConfig=new MapperConfiguration(c=>
            {
                c.AddProfile<MappingProfile>();
            });
        _mapper=mapperConfig.CreateMapper();
    }  
      
    [Fact]
    public async Task UpdateRiderProfileRequestHandler_Success()
    {
        var id = ObjectId.GenerateNewId();
        var newRider=new Rider{
             Id=id,
             FirstName="",
             LastName="",
             Email="",
             ProfilePicture="",
             PhoneNumber="251988888888"
        };
        var rid=_mockRepo.Object.Add(newRider);
        var handler = new UpdateRiderProfileRequestHandler(_mockRepo.Object);
        var request = new UpdateRiderProfileRequest
        {
            riderID = id,
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            PhoneNumber = "251987654321",
            Email = "updatedemail@rideshare.com"
        };

        var result = await handler.Handle(request, CancellationToken.None);
        var rider= await _mockRepo.Object.Get(id);
        result.ShouldNotBeNull();
        result.Succeeded.ShouldBeTrue();
        result.Message.ShouldBe("updated successfully");

        rider.FirstName.ShouldBe(request.FirstName);
        rider.LastName.ShouldBe(request.LastName);
        rider.PhoneNumber.ShouldBe(request.PhoneNumber);
        rider.Email.ShouldBe(request.Email);
        
    }

    [Fact]
    public async Task UpdateRiderProfileRequestHandler_Success_with_one_or_more_empty_value()
    {
        var id = ObjectId.GenerateNewId();
        var newRider=new Rider{
             Id=id,
             FirstName="",
             LastName="",
             Email="",
             ProfilePicture="",
             PhoneNumber="251988888888"
        };
        var rid=_mockRepo.Object.Add(newRider);
        var handler = new UpdateRiderProfileRequestHandler(_mockRepo.Object);
        var request = new UpdateRiderProfileRequest
        {
            riderID = id,
            FirstName = "UpdatedFirstName",
            LastName = "",
            PhoneNumber = "251987654321",
            Email = "updatedemail@rideshare.com"
        };

        var result = await handler.Handle(request, CancellationToken.None);
        var rider= await _mockRepo.Object.Get(id);
        result.ShouldNotBeNull();
        result.Succeeded.ShouldBeTrue();
        result.Message.ShouldBe("updated successfully");

        rider.FirstName.ShouldBe(request.FirstName);
        rider.PhoneNumber.ShouldBe(request.PhoneNumber);
        rider.Email.ShouldBe(request.Email);
        
    }   

    [Fact]
    public async Task UpdateRiderProfileRequestHandler_Should_respond_Failure()
    {
        var handler = new UpdateRiderProfileRequestHandler(_mockRepo.Object);
        var request = new UpdateRiderProfileRequest
        {
            riderID = ObjectId.GenerateNewId(),
            FirstName = "", 
            LastName = "",
            PhoneNumber = "",
            Email = ""
        };

        var result = await handler.Handle(request, CancellationToken.None);
        result.Succeeded.ShouldBeFalse();
        result.Errors.ShouldNotBeNull();
        result.Message.ShouldBe("Rider not found");
    }
}

