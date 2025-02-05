using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using AutoMapper;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.Application.Features.Payment.CQRS.Requests.Queries;
using Rideshare.Application.Features.Payment.Dtos;

namespace Rideshare.Application.Features.Payment.CQRS.Handlers;

public class GetTransactionHistoryRequestHandler : IRequestHandler<GetTransactionHistoryRequest, BaseResponse<List<GetTransactionHistoryResponseDto>>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;
    public GetTransactionHistoryRequestHandler(IMapper mapper, IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<GetTransactionHistoryResponseDto>>> Handle(GetTransactionHistoryRequest request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<GetTransactionHistoryResponseDto>>();
        var transactions = await _paymentRepository.GetAll();
        var filteredPackages = transactions.Where(p => p.UserId == request.GetTransactionHistoryRequestDto.RiderId).ToList();
        var transactionResponse = _mapper.Map<List<GetTransactionHistoryResponseDto>>(filteredPackages);
        response.IsSuccess = true;
        response.Message = "Transaction History Retrived Successfully";
        response.Value = transactionResponse;
        return response;
    }
}


