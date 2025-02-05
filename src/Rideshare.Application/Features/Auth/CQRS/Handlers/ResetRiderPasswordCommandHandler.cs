using AutoMapper;
using FluentValidation;
using MediatR;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Contracts.Identity.Models;
using Rideshare.Application.Contracts.Identity.Services;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Auth.CQRS.Commands;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Features.Auth.Dtos.Validators;
using Rideshare.Application.Responses;
using Rideshare.Application.Services;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Auth.CQRS.Handlers
{
    public class ResetRiderPasswordCommandHandler : IRequestHandler<ResetRiderPasswordCommand, BaseCommandResponse<ResetRiderPasswordResponseDto>>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ISmsService _smsService;
        private readonly IUnitOfWork _unitOfWork;

        public ResetRiderPasswordCommandHandler(IAuthService authService, IMapper mapper, ISmsService smsService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _mapper = mapper;
            _smsService = smsService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse<ResetRiderPasswordResponseDto>> Handle(ResetRiderPasswordCommand request, CancellationToken cancellationToken)
        {
            // Validate the DTO
            var validator = new ResetRiderPasswordDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ResetRiderPasswordDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseCommandResponse<ResetRiderPasswordResponseDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    "Validation Error"
                );
            }

            // Find the user by phone number
            User user = await _authService.FindRider(
                phoneNumber: request.ResetRiderPasswordDto.PhoneNumber
            );

            // If the user is not found, return an error response
            if (user == null)
            {
                return BaseCommandResponse<ResetRiderPasswordResponseDto>.Failure(
                    new List<string> { "User not found." },
                    "Password cannot be reset."
                );
            }

            // Generate OTP and send SMS
            string otpCode = new OtpGenerationService().GenerateOTP();
            
            if (user.PhoneNumber is null || !await _smsService.SendSMS(user.PhoneNumber, "Your OTP is: " + otpCode))
            {
                return BaseCommandResponse<ResetRiderPasswordResponseDto>.Failure(
                    new List<string> { "OTP code hasn't been sent. Try again!" },
                    "Password cannot be reset."
                );
            }

            try
            {
                // Save OTP in the database
                await _unitOfWork.OtpRepository.Add(new Otp { PhoneNumber = user.PhoneNumber, OtpCode = otpCode });

                // Check if the OTP is valid
                bool isOtpValidated = await _unitOfWork.OtpRepository.ValidateOtp(user?.PhoneNumber ?? "", otpCode);

                if (!isOtpValidated)
                {
                    return BaseCommandResponse<ResetRiderPasswordResponseDto>.Failure(
                        new List<string> { "OTP code is invalid. Request for another OTP and try again!" },
                        "Password cannot be reset."
                    );
                }

                // Reset the password
                ResetRiderPasswordResponse resetPasswordResponse = await _authService.ResetRiderPassword(
                    new ResetRiderPasswordRequest
                    {
                        PhoneNumber = user?.PhoneNumber,
                        NewPassword = request.ResetRiderPasswordDto.NewPassword
                    });

                return BaseCommandResponse<ResetRiderPasswordResponseDto>.Success(
                    new ResetRiderPasswordResponseDto { },
                    "Password has been reset successfully."
                );
            }
            catch (BadRequestException exception)
            {
                return BaseCommandResponse<ResetRiderPasswordResponseDto>.Failure(
                    new List<string> { exception.Message },
                    "Password cannot be reset."
                );
            }
        }
    }
}
