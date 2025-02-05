using AutoMapper;
using FluentValidation;
using MediatR;
using Rideshare.Application.Contracts.Identity.Services;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Auth.CQRS.Commands;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Features.Auth.Dtos.Validators;
using Rideshare.Application.Responses;
using Rideshare.Application.Services;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Auth.CQRS.Handlers;

public class RequestOtpForRegistrationCommandHandler : IRequestHandler<RequestNewOtpCommand, BaseCommandResponse<RequestOtpResponseDto>>
{
    private readonly IAuthService _authService;
    private readonly ISmsService _smsService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RequestOtpForRegistrationCommandHandler(IAuthService authService, ISmsService smsService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _authService = authService;
        _smsService = smsService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseCommandResponse<RequestOtpResponseDto>> Handle(RequestNewOtpCommand request, CancellationToken cancellationToken)
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

        // Check if the user already exists
        var user = await _authService.FindUser(request.RequestOtpDto.PhoneNumber);
        if (user != null)
        {
            return BaseCommandResponse<RequestOtpResponseDto>.Failure(
                new List<string> { "Phone number is already registered." },
                "Registration failed, Phone number is already registered."
            );
        }

        // Generate OTP and send SMS
        string otpCode = new OtpGenerationService().GenerateOTP();
        if (!await _smsService.SendSMS(request.RequestOtpDto.PhoneNumber, "Your OTP is: " + otpCode))
        {
            return BaseCommandResponse<RequestOtpResponseDto>.Failure(
                new List<string> { "OTP code hasn't been sent. Try again!" },
                "Registration failed."
            );
        }

        try
        {
            // Save OTP in the database
            await _unitOfWork.OtpRepository.Add(new Otp { PhoneNumber = request.RequestOtpDto.PhoneNumber, OtpCode = otpCode });

            return BaseCommandResponse<RequestOtpResponseDto>.Success(
                new RequestOtpResponseDto { },
                "OTP Code has been sent successfully."
            );
        }
        catch (Exception exception)
        {
            return BaseCommandResponse<RequestOtpResponseDto>.Failure(
                new List<string> { exception.Message },
                "Registration failed."
            );
        }
    }
}
