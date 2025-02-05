using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.Application.Features.Payment.Dtos;

namespace Rideshare.Application.Features.Payment.CQRS.Requests.Queries;

public class GetTransactionHistoryRequest  : IRequest<BaseResponse<List<GetTransactionHistoryResponseDto>>>
{
    public required GetTransactionHistoryRequestDto GetTransactionHistoryRequestDto {set; get;}
}
