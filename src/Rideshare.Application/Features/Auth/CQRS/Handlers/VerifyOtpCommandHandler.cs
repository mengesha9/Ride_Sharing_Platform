using FluentValidation;
using MediatR;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Contracts.Identity.Models;
using Rideshare.Application.Contracts.Identity.Services;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Auth.CQRS.Commands;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Features.Auth.Dtos.Validators;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Handlers
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, BaseCommandResponse<VerifyOtpResponse>>
    {
        private readonly IRiderRepository _riderRepository;
        private readonly IOtpRepository _otpRepository;
        private readonly IAuthService _authService;

        public VerifyOtpCommandHandler(IRiderRepository riderRepository, IOtpRepository otpRepository, IAuthService authService)
        {
            _riderRepository = riderRepository;
            _otpRepository = otpRepository;
            _authService = authService;
        }

        public async Task<BaseCommandResponse<VerifyOtpResponse>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            // Validate the request DTO
            var validator = new VerifyOtpRequestValidator();
            var validationResult = await validator.ValidateAsync(request.VerifyOtpDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseCommandResponse<VerifyOtpResponse>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    "Validation failed."
                );
            }

            var response = new BaseCommandResponse<VerifyOtpResponse>();
            try
            {
                var verificationResponse = await _otpRepository.ValidateOtp(request.VerifyOtpDto.PhoneNumber, request.VerifyOtpDto.OtpCode);
                if (verificationResponse)
                {
                    // Optionally call the AuthService to verify phone number
                    // await _authService.VerifyPhoneNumber(
                    //     new VerifyPhoneNumberRequest { PhoneNumber = request.VerifyOtpDto.PhoneNumber }
                    // );

                    response.Succeeded = true;
                    response.Message = "Phone number verified successfully.";
                }
                else
                {
                    throw new BadRequestException("Invalid OTP code.");
                }
                return response;
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message, request.VerifyOtpDto.PhoneNumber);
            }
            catch (BadRequestException ex)
            {
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
