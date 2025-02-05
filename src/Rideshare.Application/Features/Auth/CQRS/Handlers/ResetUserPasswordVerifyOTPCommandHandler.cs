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
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Auth.CQRS.Handlers
{
    public class ResetUserPasswordVerifyOTPCommandHandler : IRequestHandler<ResetUserPasswordVerifyOtpCommand, BaseCommandResponse<ResetUserPasswordVerifyOTPResponseDto>>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ISmsService _smsService;
        private readonly IUnitOfWork _unitOfWork;

        public ResetUserPasswordVerifyOTPCommandHandler(IAuthService authService, IMapper mapper, ISmsService smsService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _mapper = mapper;
            _smsService = smsService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse<ResetUserPasswordVerifyOTPResponseDto>> Handle(ResetUserPasswordVerifyOtpCommand request, CancellationToken cancellationToken)
        {
            // Validate the request DTO
            var validator = new ResetUserPasswordVerifyOTPDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ResetUserPasswordVerifyOtpDTO, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseCommandResponse<ResetUserPasswordVerifyOTPResponseDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    "Validation failed."
                );
            }

            // Continue with the original logic
            User user = await _authService.FindUser(
                phoneNumber: request.ResetUserPasswordVerifyOtpDTO.PhoneNumber
            );
            string otpCode = request.ResetUserPasswordVerifyOtpDTO.OTP;

            try
            {
                bool isOtpValidated = await _unitOfWork.OtpRepository.ValidateOtp(user?.PhoneNumber ?? "", otpCode);

                if (!isOtpValidated)
                {
                    return BaseCommandResponse<ResetUserPasswordVerifyOTPResponseDto>.Failure(
                        new List<string> { "OTP code is invalid. Request for another OTP and try again!" },
                        "Password cannot be reset."
                    );
                }

                ResetPasswordResponse resetPasswordResponse = await _authService.ResetPassword(
                    new ResetPasswordRequest
                    {
                        Email = user?.Email,
                        Username = user?.UserName,
                        PhoneNumber = user?.PhoneNumber,
                    });

                return BaseCommandResponse<ResetUserPasswordVerifyOTPResponseDto>.Success(
                    new ResetUserPasswordVerifyOTPResponseDto
                    {
                        PasswordResetToken = resetPasswordResponse.Token
                    },
                    "Password reset token generated successfully."
                );
            }
            catch (BadRequestException exception)
            {
                return BaseCommandResponse<ResetUserPasswordVerifyOTPResponseDto>.Failure(
                    new List<string> { exception.Message },
                    "Invalid OTP code."
                );
            }
        }
    }
}
