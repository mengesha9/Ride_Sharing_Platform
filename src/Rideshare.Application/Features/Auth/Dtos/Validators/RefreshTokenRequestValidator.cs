using FluentValidation;

namespace Rideshare.Application.Features.Auth.Dtos.Validators
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(dto => dto.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken is required.");
        }
        
    }
}