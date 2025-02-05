using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Riders.Dtos;

namespace Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
public class GetRiderListRequest : IRequest<BaseResponse<List<GetRidersListResponseDto>>>
{
    public GetRidersListRequestDto GetRidersListRequestDto { set; get; }
}