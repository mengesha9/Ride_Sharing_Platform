using AutoMapper;
using FluentValidation;
using MediatR;
using Rideshare.Application.Common.Exceptions;
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
    public class RequestOtpCommandHandler : IRequestHandler<RequestOtpCommand, BaseCommandResponse<RequestOtpResponseDto>>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ISmsService _smsService;
        private readonly IUnitOfWork _unitOfWork;

        public RequestOtpCommandHandler(IAuthService authService, IMapper mapper, ISmsService smsService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _mapper = mapper;
            _smsService = smsService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse<RequestOtpResponseDto>> Handle(RequestOtpCommand request, CancellationToken cancellationToken)
        {
            // Validate the DTO
            var validator = new RequestOtpDtoValidator();
            var validationResult = await validator.ValidateAsync(request.RequestOtpDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseCommandResponse<RequestOtpResponseDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    "Validation Error"
                );
            }

            User user = await _authService.FindUser(
                phoneNumber: request.RequestOtpDto.PhoneNumber
            );

            if (user == null)
            {
                return BaseCommandResponse<RequestOtpResponseDto>.Failure(
                    new List<string> { "User not found." },
                    "User not found."
                );
            }

            string otpCode = new OtpGenerationService().GenerateOTP();
            if (user.PhoneNumber is null || !await _smsService.SendSMS(user.PhoneNumber, "Your OTP is: " + otpCode))
            {
                return BaseCommandResponse<RequestOtpResponseDto>.Failure(
                    new List<string> { "OTP code hasn't been sent. Try again!" },
                    "Password cannot be reset."
                );
            }

            try
            {
                await _unitOfWork.OtpRepository.Add(new Otp { PhoneNumber = user.PhoneNumber, OtpCode = otpCode });

                return BaseCommandResponse<RequestOtpResponseDto>.Success(
                    new RequestOtpResponseDto { },
                    "OTP Code has been sent successfully."
                );
            }
            catch (BadRequestException exception)
            {
                return BaseCommandResponse<RequestOtpResponseDto>.Failure(
                    new List<string> { exception.Message },
                    "Password cannot be reset."
                );
            }
        }
    }
}
