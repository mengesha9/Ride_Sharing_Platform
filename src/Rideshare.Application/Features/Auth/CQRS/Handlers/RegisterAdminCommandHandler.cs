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

namespace Rideshare.Application.Features.Auth.CQRS.Handlers
{
    public class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand, BaseCommandResponse<RegisterAdminResponseDto>>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterAdminCommandHandler(IAuthService authService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse<RegisterAdminResponseDto>> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
        {
            // Validate the DTO
            var validator = new RegisterAdminDtoValidator();
            var validationResult = validator.Validate(request.RegisterAdminDto);

            if (!validationResult.IsValid)
            {
                return BaseCommandResponse<RegisterAdminResponseDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    "Admin could not be registered due to validation errors."
                );
            }

            var registerUserRequest = _mapper.Map<RegisterUserRequest>(request.RegisterAdminDto);
            registerUserRequest.Roles = new List<string> { "Admin" };

            try
            {
                // Register the user with the auth service
                var registerUserResponse = await _authService.RegisterUser(registerUserRequest);
                var user = registerUserResponse.User;

                // Add the admin entity to the database
                var admin = _unitOfWork.AdminRepository.Add(new Admin
                {
                    ApplicationUserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });

                var registerAdminResponseDto = new RegisterAdminResponseDto
                {
                    Token = registerUserResponse.Token,
                    User = _mapper.Map<UserDto>(user),
                };

                return BaseCommandResponse<RegisterAdminResponseDto>.Success(registerAdminResponseDto, "Admin registered successfully.");
            }
            catch (BadRequestException exception)
            {
                return BaseCommandResponse<RegisterAdminResponseDto>.Failure(
                    new List<string> { exception.Message },
                    "Admin could not be created."
                );
            }
        }
    }
}
