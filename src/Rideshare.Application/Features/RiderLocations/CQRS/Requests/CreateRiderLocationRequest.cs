using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.RiderLocations.Dtos;

namespace Rideshare.Application.Features.RiderLocations.CQRS.Requests;

public class CreateRiderLocationRequest : IRequest<BaseCommandResponse>
{
    public CreateRiderLocationRequestDto CreateRiderLocationRequestDto {set; get;}
}
