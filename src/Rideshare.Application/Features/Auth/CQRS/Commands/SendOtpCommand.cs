using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Auth.Dtos;

namespace Rideshare.Application.Features.Auth.CQRS.Commands;

public class SendOtpCommand: IRequest<BaseCommandResponse>
{
     public SendOtpRequest sendOtpRequest {get; set;}
}
