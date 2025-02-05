using FluentValidation;
using MediatR;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Contracts.Identity.Models;
using Rideshare.Application.Contracts.Identity.Services;
using Rideshare.Application.Features.Auth.CQRS.Commands;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Features.Auth.Dtos.Validators;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Handlers
{
    public class ResetUserPasswordVerifyCommandHandler : IRequestHandler<ResetUserPasswordVerifyCommand, BaseCommandResponse<ResetUserPasswordVerifyResponseDto>>
    {
        private readonly IAuthService _authService;

        public ResetUserPasswordVerifyCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<BaseCommandResponse<ResetUserPasswordVerifyResponseDto>> Handle(ResetUserPasswordVerifyCommand request, CancellationToken cancellationToken)
        {
            // Instantiate and validate using the validator
            var validator = new ResetUserPasswordVerifyDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ResetUserPasswordVerifyDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseCommandResponse<ResetUserPasswordVerifyResponseDto>.Failure(
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    "Validation failed."
                );
            }

            try
            {
                var resetPasswordVerifyRequest = new ResetPasswordVerifyRequest
                {
                    Username = request.ResetUserPasswordVerifyDto.Username,
                    Email = request.ResetUserPasswordVerifyDto.Email,
                    PhoneNumber = request.ResetUserPasswordVerifyDto.PhoneNumber,
                    Token = request.ResetUserPasswordVerifyDto.Token,
                    NewPassword = request.ResetUserPasswordVerifyDto.NewPassword
                };

                await _authService.ResetPasswordVerify(resetPasswordVerifyRequest);

                return BaseCommandResponse<ResetUserPasswordVerifyResponseDto>.Success(
                    new ResetUserPasswordVerifyResponseDto(),
                    "Password has been reset successfully."
                );
            }
            catch (BadRequestException exception)
            {
                return BaseCommandResponse<ResetUserPasswordVerifyResponseDto>.Failure(
                    new List<string> { exception.Message },
                    "Password could not be reset."
                );
            }
            catch (Exception exception)
            {
                return BaseCommandResponse<ResetUserPasswordVerifyResponseDto>.Failure(
                    new List<string> { exception.Message },
                    "An unexpected error occurred while resetting the password."
                );
            }
        }
    }
}
