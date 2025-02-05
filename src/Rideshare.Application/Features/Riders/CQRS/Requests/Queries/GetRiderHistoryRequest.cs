using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Riders.CQRS.Requests.Queries;

public class GetRiderHistoryRequest : IRequest<BaseResponse<List<GetRiderHistoryResponseDto>>>
{
    public required GetRiderHistoryRequestDto GetRiderHistoryRequestDto { set; get; }
}
