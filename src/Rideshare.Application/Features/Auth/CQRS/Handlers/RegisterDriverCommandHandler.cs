using AutoMapper;
using FluentValidation;
using MediatR;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Common.Response;
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
    public class RegisterDriverCommandHandler : IRequestHandler<RegisterDriverCommand, BaseCommandResponse<RegisterDriverResponseDto>>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsService _smsService;

        public RegisterDriverCommandHandler(ISmsService smsService, IAuthService authService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _smsService = smsService;
        }

        public async Task<BaseCommandResponse<RegisterDriverResponseDto>> Handle(RegisterDriverCommand request, CancellationToken cancellationToken)
        {
            // Validate the RegisterDriverDto
            var validator = new RegisterDriverDtoValidator();
            var validationResult = await validator.ValidateAsync(request.RegisterDriverDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseCommandResponse<RegisterDriverResponseDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    "Driver could not be created due to validation errors."
                );
            }

            var registerUserRequest = _mapper.Map<RegisterUserRequest>(request.RegisterDriverDto);
            registerUserRequest.Roles = new List<string> { "Driver" };

            try
            {
                var registerUserResponse = await _authService.RegisterUser(registerUserRequest);
                var user = registerUserResponse.User;

                string otpCode = new OtpGenerationService().GenerateOTP();
                var sentSms = await _smsService.SendSMS(request.RegisterDriverDto.PhoneNumber, otpCode);

                if (!sentSms)
                {
                    return BaseCommandResponse<RegisterDriverResponseDto>.Failure(
                        new List<string> { "OTP code hasn't been sent. Try again!" },
                        "Driver could not be created."
                    );
                }

                await _unitOfWork.OtpRepository.Add(new Otp { PhoneNumber = request.RegisterDriverDto.PhoneNumber, OtpCode = otpCode });

                var driver = new Driver { ApplicationUserId = user.Id };
                await _unitOfWork.DriverRepository.Add(driver);

                var registerDriverResponseDto = new RegisterDriverResponseDto
                {
                    Token = registerUserResponse.Token,
                    User = _mapper.Map<UserDto>(user),
                };

                return BaseCommandResponse<RegisterDriverResponseDto>.Success(registerDriverResponseDto, "OTP code has been sent successfully");
            }
            catch (BadRequestException exception)
            {
                return BaseCommandResponse<RegisterDriverResponseDto>.Failure(
                    new List<string> { exception.Message },
                    "Driver could not be created."
                );
            }
        }
    }
}
