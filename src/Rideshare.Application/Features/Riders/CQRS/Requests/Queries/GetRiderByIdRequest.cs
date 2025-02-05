using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Riders.Dtos;

namespace Rideshare.Application.Features.Riders.CQRS.Requests.Queries;

public class GetRiderByIdRequest : IRequest<BaseResponse<GetRiderByIdResponseDto>>
{
  public GetRiderByIdRequestDto GetRiderByIdRequestDto { set; get; }
}