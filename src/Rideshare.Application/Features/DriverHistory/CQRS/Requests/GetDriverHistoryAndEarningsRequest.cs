using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.DriverHistory.Dtos;

public class GetDriverHistoryAndEarningsRequest : IRequest<BaseResponse<List<GetDriverHIstoryAndEarningsResponseDto>>>
{
    public required GetDriverHIstoryAndEarningsRequestDto GetDriverHIstoryAndEarningsRequestDto { set; get; }

}