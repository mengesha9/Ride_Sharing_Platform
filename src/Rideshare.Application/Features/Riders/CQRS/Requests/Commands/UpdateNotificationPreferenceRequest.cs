using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Riders.Dtos;

namespace Rideshare.Application.Features.Riders.Requests.Commands;

public class UpdateNotificationPreferenceRequest : IRequest<BaseCommandResponse>
{
    public UpdateNotificationPreferenceRequestDto  UpdateNotificationPreferenceRequestDto {set; get;}
}
