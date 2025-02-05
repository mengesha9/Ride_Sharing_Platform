using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Riders.Dtos;

namespace Rideshare.Application.Features.Riders.Requests.Commands;

public class UpdateRiderDeviceTokenRequest : IRequest<BaseCommandResponse>
{
    public UpdateRiderDeviceTokenRequestDto  UpdateRiderDeviceTokenRequestDto {set; get;}
}
