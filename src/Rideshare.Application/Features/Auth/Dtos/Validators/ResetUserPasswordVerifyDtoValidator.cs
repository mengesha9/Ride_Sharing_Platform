using FluentValidation;

namespace Rideshare.Application.Features.Auth.Dtos.Validators
{
    public class ResetUserPasswordVerifyDtoValidator : AbstractValidator<ResetUserPasswordVerifyDto>
    {
        public ResetUserPasswordVerifyDtoValidator()
        {
             RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .When(x => string.IsNullOrEmpty(x.Email) && string.IsNullOrEmpty(x.Username))
                .Matches(@"^(251)(9|7)\d{8}$").WithMessage("Phone number is not valid, Phone Number must be in the format 251 followed by 9 or 7 and then 8 digits.")
                .MaximumLength(12).WithMessage("Phone number must not exceed 12 characters.")
                .Unless(x => string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .When(x => string.IsNullOrEmpty(x.PhoneNumber) && string.IsNullOrEmpty(x.Username))
                .EmailAddress().WithMessage("Invalid email format.")
                .Unless(x => string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .When(x => string.IsNullOrEmpty(x.PhoneNumber) && string.IsNullOrEmpty(x.Email))
                .Unless(x => string.IsNullOrEmpty(x.Username));

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
        }
    }
}