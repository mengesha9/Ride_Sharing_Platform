using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Auth.Dtos;

namespace Rideshare.Application.Features.Auth.CQRS.Commands;


public class VerificationCommand :  IRequest<BaseResponse<VerifyOtpResponse>>
{
    public VerifyOtpRequest verifyOtpRequest {set; get;}
}