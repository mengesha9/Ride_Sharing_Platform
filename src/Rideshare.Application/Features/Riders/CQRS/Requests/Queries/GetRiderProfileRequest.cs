using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Riders.Dtos;


namespace Rideshare.Application.Features.Riders.CQRS.Requests.Queries;

public class GetRiderProfileRequest: IRequest<BaseResponse<GetRiderProfileResponseDto>>
{
    public GetRiderProfileRequestDto GetRiderProfileRequestDto {set; get;}
}
