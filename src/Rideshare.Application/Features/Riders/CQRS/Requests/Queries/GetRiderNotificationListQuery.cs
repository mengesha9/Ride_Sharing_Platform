using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Riders.Dtos;

namespace Rideshare.Application.Features.Riders.CQRS.Requests.Queries
{
    public class GetRiderNotificationListQuery : IRequest<BaseResponse<List<GetRiderNotifcationsListResponseDto>>>
    {
        public GetRiderNotifcationsListRequestDto GetRiderNotifcationsListRequestDto { get; set; }
    }
}