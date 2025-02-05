using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.PaymentSystem.Application.Dtos;

namespace Rideshare.Application.Features.PurchasePackage.CQRS.Command

{
    public class ProcessPaymentCommand : IRequest<BaseResponse<ChapaResponseDto>>
{
    public ChapaRequestDto RequestDto { get; set; }

    // public ProcessPaymentCommand(ChapaRequestDto requestDto)
    // {
    //     RequestDto = requestDto;
    // }
}

}