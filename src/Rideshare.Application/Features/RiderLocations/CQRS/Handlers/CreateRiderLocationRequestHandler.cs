using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RiderLocations.CQRS.Requests;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.RiderLocations.CQRS.Handlers;

public class CreateRiderLocationRequestHandler : IRequestHandler<CreateRiderLocationRequest, BaseCommandResponse>
{
  private readonly IRiderLocationRepository _riderLocationRepository;
  public CreateRiderLocationRequestHandler(IRiderLocationRepository riderLocationRepository)
  {
    _riderLocationRepository = riderLocationRepository;
  }
  public async Task<BaseCommandResponse> Handle(CreateRiderLocationRequest request, CancellationToken cancellationToken)
  {
    var response = new BaseCommandResponse();
    await _riderLocationRepository.Add(new RiderLocation
    {
      Id = new ObjectId(),
      RiderId = request.CreateRiderLocationRequestDto.RiderId,
      Location = request.CreateRiderLocationRequestDto.Location,
      Name = request.CreateRiderLocationRequestDto.Name
    });
    response.IsSuccess = true;
    return response;
  }
}
