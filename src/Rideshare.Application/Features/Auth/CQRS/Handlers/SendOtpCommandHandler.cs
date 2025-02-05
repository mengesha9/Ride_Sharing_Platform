using AutoMapper;
using FluentValidation;
using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Auth.CQRS.Commands;
using Rideshare.Application.Features.Auth.Dtos.Validators;
using Rideshare.Application.Services;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Auth.CQRS.Handlers
{
    public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, BaseCommandResponse>
    {
        private readonly ISmsService _smsService;
        private readonly IRiderRepository _riderRepository;
        private readonly IOtpRepository _otpRepository;
        private readonly IMapper _mapper;

        public SendOtpCommandHandler(ISmsService smsService, IRiderRepository riderRepository, IOtpRepository otpRepository, IMapper mapper)
        {
            _smsService = smsService;
            _riderRepository = riderRepository;
            _otpRepository = otpRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            // Validate the DTO
            var validator = new SendOtpRequestValidator();
            var validationResult = await validator.ValidateAsync(request.sendOtpRequest, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                response.Message = "Validation Error";
                return response;
            }

            string otpCode = new OtpGenerationService().GenerateOTP();

            try
            {
                var sentSms = await _smsService.SendSMS(request.sendOtpRequest.PhoneNumber, otpCode);

                if (sentSms)
                {
                    var riderExists = await _riderRepository.ExistsByPhoneNumber(request.sendOtpRequest.PhoneNumber);

                    if (riderExists)
                    {
                        var existingValue = await _otpRepository.GetByPhoneNumber(request.sendOtpRequest.PhoneNumber);
                        existingValue.OtpCode = otpCode;
                        await _otpRepository.Update(existingValue);
                    }
                    else
                    {
                        await _riderRepository.Add(new Rider { Id = new ObjectId(), PhoneNumber = request.sendOtpRequest.PhoneNumber });
                        await _otpRepository.Add(new Otp { Id = new ObjectId(), PhoneNumber = request.sendOtpRequest.PhoneNumber, OtpCode = otpCode });
                    }

                    var rider = await _riderRepository.GetByPhoneNUmber(request.sendOtpRequest.PhoneNumber);
                    response.Id = rider.Id;
                    response.Message = "OTP code has been sent successfully";
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "OTP code hasn't been sent. Try again!";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.Errors = new List<string> { ex.Message };
                response.IsSuccess = false;
            }

            return response;
        }
    }
}
