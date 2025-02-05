using MediatR;
using Newtonsoft.Json;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.PaymentSystem.Application.Dtos;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Features.PurchasePackage.CQRS.Command;
using Rideshare.Application.Contracts.Persistence;
using MongoDB.Bson;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.PaymentSystem.CQRS.Handlers
{
    

    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, BaseResponse<ChapaResponseDto>>
    {
        private readonly IChapaService _chapaService;

        private readonly IPaymentRepository _paymentRepository;

        public ProcessPaymentCommandHandler(IChapaService chapaService, IPaymentRepository paymentRepository)
        {
            _chapaService = chapaService;
            _paymentRepository = paymentRepository;

        }


        
        public async Task<BaseResponse<ChapaResponseDto>> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            var mainResponse = new BaseResponse<ChapaResponseDto>();
            var txRef = Guid.NewGuid().ToString();
            request.RequestDto.TransactionReference = txRef;
            var response = await _chapaService.ProcessPaymentAsync(request.RequestDto);
            
            

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

            var newPayment = new Rideshare.Domain.Entities.Payment
            {
                Id = ObjectId.GenerateNewId(),
                FirstName = request.RequestDto.FirstName, 
                LastName = request.RequestDto.LastName,  
                PackageId = request.RequestDto.PackageId,
                Amount = request.RequestDto.Amount,
                IsSuccessPayment = false

            };

            await _paymentRepository.Add(newPayment);
 
            if (parsedResponse.status == "success")
            {

                mainResponse.Value = new ChapaResponseDto
                {
                    CheckoutUrl = parsedResponse.data.checkout_url,
                };
                mainResponse.IsSuccess = true;
                return mainResponse;
            }
        }
        mainResponse.IsSuccess = false;
        return mainResponse;
        }
    }

}