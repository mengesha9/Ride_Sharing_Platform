using System.Security.Authentication;
using AutoMapper;
using FluentValidation;
using MediatR;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Identity.Services;
using Rideshare.Application.Features.Auth.CQRS.Commands;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Features.Auth.Dtos.Validators;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.CQRS.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, BaseResponse<RefreshTokenResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public RefreshTokenCommandHandler(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<BaseResponse<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<RefreshTokenResponse>();

            // Validate the RefreshTokenRequest DTO
            var validator = new RefreshTokenRequestValidator();
            var validationResult = await validator.ValidateAsync(request.RefreshTokenRequest, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation failed.";
                response.Error = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var refreshTokenRequest = _mapper.Map<RefreshTokenRequest>(request.RefreshTokenRequest);
            try
            {
                var refreshTokenResponse = await _authService.RefreshToken(refreshTokenRequest, request.UserId);
                response.IsSuccess = true;
                response.Message = "Token refreshed successfully.";
                response.Value = refreshTokenResponse;
            }
            catch (AuthenticationException exception)
            {
                response.IsSuccess = false;
                response.Message = "Authentication Error";
                response.Error = new List<string> { exception.Message };
            }
            catch (BadRequestException exception)
            {
                response.IsSuccess = false;
                response.Message = "Bad Request Error";
                response.Error = new List<string> { exception.Message };
            }

            return response;
        }
    }
}
