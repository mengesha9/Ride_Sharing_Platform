using AutoMapper;
using Moq;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Riders.CQRS.Handlers.Queries;
using Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Application.Profiles;
using Rideshare.Application.Test.Mocks.Persistence;
using Shouldly;

namespace Rideshare.Application.Test.Features.Riders.CQRS.Handlers.Queries;

public class GetRiderListRequestHandlerTest
{
  private readonly IMapper _mapper;
  private readonly Mock<IRiderRepository> _riderRepository;

  public GetRiderListRequestHandlerTest()
  {
    _riderRepository = MockRiderRepository.GetMockRiderRepository();

    var mapperConfig = new MapperConfiguration(c =>
        {
          c.AddProfile<MappingProfile>();
        });

    _mapper = mapperConfig.CreateMapper();
  }

  [Fact]
  public async Task GetRiderListTest()
  {
    var handler = new GetRiderListRequestHandler(_riderRepository.Object, _mapper);
    var result = await handler.Handle(new GetRiderListRequest(), CancellationToken.None);
    result.ShouldBeOfType<BaseResponse<List<GetRidersListResponseDto>>>();
    result.Value.Count.ShouldBe(2);
  }

}