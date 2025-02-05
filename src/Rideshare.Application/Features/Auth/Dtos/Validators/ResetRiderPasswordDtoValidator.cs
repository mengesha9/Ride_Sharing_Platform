using FluentValidation;

namespace Rideshare.Application.Features.Auth.Dtos.Validators
{
    public class ResetRiderPasswordDtoValidator : AbstractValidator<ResetRiderPasswordDto>
    {
        public ResetRiderPasswordDtoValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(12).WithMessage("Phone number must not exceed 12 characters.")
                .Matches(@"^(251)(9|7)\d{8}$").WithMessage("Phone number is not valid, Phone Number must be in the format 251 followed by 9 or 7 and then 8 digits.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");
        }
        
    }
}