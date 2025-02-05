using FluentValidation;

namespace Rideshare.Application.Features.Auth.Dtos.Validators
{
    public class RegisterDriverDtoValidator : AbstractValidator<RegisterDriverDto>
    {
        public RegisterDriverDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(50).WithMessage("Full name must not exceed 50 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^(251)(9|7)\d{8}$").WithMessage("Phone number is not valid, Phone Number must be in the format 251 followed by 9 or 7 and then 8 digits.")
                .MaximumLength(12).WithMessage("Phone number must not exceed 12 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\@\!\?\*\.]").WithMessage("Password must contain at least one special character (e.g., @, !, ?, *, .).");
        }
    }
}