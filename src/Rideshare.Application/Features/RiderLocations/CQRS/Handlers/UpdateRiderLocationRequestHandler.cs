using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.RiderLocations.CQRS.Requests;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.RiderLocations.CQRS.Handlers;

public class UpdateRiderLocationRequestHandler : IRequestHandler<UpdateRiderLocationRequest, BaseCommandResponse>
{
  private readonly IRiderLocationRepository _riderLocationRepository;
  public UpdateRiderLocationRequestHandler(IRiderLocationRepository riderLocationRepository)
  {
    _riderLocationRepository = riderLocationRepository;
  }
  public async Task<BaseCommandResponse> Handle(UpdateRiderLocationRequest request, CancellationToken cancellationToken)
  {
    var response = new BaseCommandResponse();
    await _riderLocationRepository.Update(new RiderLocation
    {
      Id = request.Id,
      RiderId = request.RiderId,
      Location = request.Location,
      Name = request.Name
    });
    response.IsSuccess = true;
    return response;
  }
}
