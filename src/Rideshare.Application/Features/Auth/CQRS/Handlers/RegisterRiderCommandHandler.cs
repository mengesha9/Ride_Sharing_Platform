using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Contracts.Identity.Models;
using Rideshare.Application.Contracts.Identity.Services;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Auth.CQRS.Commands;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Features.Auth.Dtos.Validators;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using FluentValidation;
using System.Linq;
using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.Auth.CQRS.Handlers;

public class RegisterRiderCommandHandler : IRequestHandler<RegisterRiderCommand, BaseCommandResponse<RegisterRiderResponseDto>>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterRiderCommandHandler(IAuthService authService, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseCommandResponse<RegisterRiderResponseDto>> Handle(RegisterRiderCommand request, CancellationToken cancellationToken)
    {
        // Validate the DTO
        var validator = new RegisterRiderDtoValidator();
        var validationResult = await validator.ValidateAsync(request.RegisterRiderDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BaseCommandResponse<RegisterRiderResponseDto>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToList(), 
                "Validation Error"
            );
        }

        // Check if the rider is already registered
        var riderExists = await _unitOfWork.RiderRepository.ExistsByPhoneNumber(request.RegisterRiderDto.PhoneNumber);
        if (riderExists)
        {
            throw new ConflictException("Rider is already registered with this phone number.");
        }

        var registerUserRequest = _mapper.Map<RegisterUserRequest>(request.RegisterRiderDto);
        registerUserRequest.Roles = new List<string> { "Rider" };

        try
        {
            var registerUserResponse = await _authService.RegisterUser(registerUserRequest);
            var user = registerUserResponse.User;

            var rider = new Rider
            {
                ApplicationUserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber ?? "",
                Email = user.Email ?? "",
                UpdatedAt = DateTime.UtcNow
            };
            await _unitOfWork.RiderRepository.Add(rider);

            var registerRiderResponseDto = new RegisterRiderResponseDto
            {
                Token = registerUserResponse.Token,
                RefreshToken = registerUserResponse.RefreshToken,
                User = _mapper.Map<UserDto>(user),
            };

            return BaseCommandResponse<RegisterRiderResponseDto>.Success(registerRiderResponseDto, "Rider registered successfully.");
        }
        catch (BadRequestException exception)
        {
            return BaseCommandResponse<RegisterRiderResponseDto>.Failure(
                new List<string> { exception.Message }, 
                "Rider could not be created."
            );
        }
    }
}
