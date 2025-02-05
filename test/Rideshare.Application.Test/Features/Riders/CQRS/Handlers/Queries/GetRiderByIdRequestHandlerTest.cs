using System.Linq.Expressions;
using AutoMapper;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using Moq;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Riders.CQRS.Handlers.Queries;
using Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Persistence;
using Shouldly;

namespace Rideshare.Application.Test.Features.Riders.CQRS.Handlers.Queries;

public class GetRiderByIdRequestHandlerTest
{
  private readonly Mock<IRiderRepository> _riderRepository;
  private readonly IMapper _mapper;

  public GetRiderByIdRequestHandlerTest()
  {
    _riderRepository = MockRiderRepository.GetMockRiderRepository();

    var mapperConfig = new MapperConfiguration(c =>
    {
      c.AddProfile<MappingProfile>();
    });

    _mapper = mapperConfig.CreateMapper();
  }

  [Fact]
  public async Task GetRiderByIdRequestHandler_Success()
  {
    //Arrange
    var riders = await new GetRiderListRequestHandler(_riderRepository.Object, _mapper).Handle(new GetRiderListRequest(), CancellationToken.None);
    var validRiderIdA = riders.Value[0].Id;
    var validRiderIdB = riders.Value[1].Id;

    //Act
    var handler = new GetRiderByIdRequestHandler(_riderRepository.Object, _mapper);
    var resultA = await handler.Handle(new GetRiderByIdRequest { GetRiderByIdRequestDto = new GetRiderByIdRequestDto { RiderId = validRiderIdA } }, CancellationToken.None);
    var resultB = await handler.Handle(new GetRiderByIdRequest { GetRiderByIdRequestDto = new GetRiderByIdRequestDto { RiderId = validRiderIdB } }, CancellationToken.None);

    //Assert
    resultA.ShouldBeOfType<BaseResponse<GetRiderByIdResponseDto>>();
    resultA.IsSuccess.ShouldBeTrue();
    Assert.NotNull(resultA.Value);
    Assert.Equal(validRiderIdA, resultA.Value.Id);
    Assert.Equal("Rider 1", resultA.Value.FirstName);
  }

  [Fact]
  public async Task GetRiderByIdRequestHandler_Failure()
  {
    //Arrange
    var invalidRiderId = ObjectId.Parse("000000000000000000000001");

    var handler = new GetRiderByIdRequestHandler(_riderRepository.Object, _mapper);
    //Act
    var invalidResult = await handler.Handle(new GetRiderByIdRequest { GetRiderByIdRequestDto = new GetRiderByIdRequestDto { RiderId = invalidRiderId } }, CancellationToken.None);

    // Invalid Assert
    invalidResult.ShouldBeOfType<BaseResponse<GetRiderByIdResponseDto>>();
    invalidResult.IsSuccess.ShouldBeFalse();
    invalidResult.Message.ShouldBe("Rider not found.");
    Assert.Null(invalidResult.Value);
  }
}