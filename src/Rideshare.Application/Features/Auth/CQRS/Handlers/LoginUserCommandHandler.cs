using AutoMapper;
using FluentValidation;
using MediatR;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Contracts.Identity.Models;
using Rideshare.Application.Contracts.Identity.Services;
using Rideshare.Application.Features.Auth.CQRS.Commands;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Features.Auth.Dtos.Validators;
using Rideshare.Application.Responses;
using System.Linq;

namespace Rideshare.Application.Features.Auth.CQRS.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, BaseCommandResponse<LoginResponseDto>>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse<LoginResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Validate the DTO
            var validator = new LoginUserDtoValidator();
            var validationResult = await validator.ValidateAsync(request.LoginUserDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseCommandResponse<LoginResponseDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList(), 
                    "Validation Error"
                );
            }

            var loginUserRequest = _mapper.Map<LoginUserRequest>(request.LoginUserDto);

            try
            {
                var loginUserResponse = await _authService.LoginUser(loginUserRequest);

                var loginResponseDto = new LoginResponseDto
                {
                    Token = loginUserResponse.Token,
                    RefreshToken = loginUserResponse.RefreshToken,
                    User = _mapper.Map<UserDto>(loginUserResponse.User)
                };

                return BaseCommandResponse<LoginResponseDto>.Success(loginResponseDto, "User logged in successfully.");
            }
            catch (NotFoundException exception)
            {
                return BaseCommandResponse<LoginResponseDto>.Failure(new List<string> { exception.Message }, "User could not be logged in.");
            }
            catch (BadRequestException exception)
            {
                return BaseCommandResponse<LoginResponseDto>.Failure(new List<string> { exception.Message }, "User could not be logged in.");
            }
        }
    }
}
