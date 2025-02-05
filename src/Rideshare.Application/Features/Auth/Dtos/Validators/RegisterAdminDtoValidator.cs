using FluentValidation;

namespace Rideshare.Application.Features.Auth.Dtos.Validators
{
    public class RegisterAdminDtoValidator : AbstractValidator<RegisterAdminDto>
    {
        public RegisterAdminDtoValidator()
        {
            RuleFor(dto => dto.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters long.");

            RuleFor(dto => dto.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.");

            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.")
                .MaximumLength(50).WithMessage("Email must not exceed 50 characters.");

            RuleFor(dto => dto.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^(251)(9|7)\d{8}$").WithMessage("Phone number is not valid, Phone Number must be in the format 251 followed by 9 or 7 and then 8 digits.")
                .MaximumLength(12).WithMessage("Phone number must not exceed 12 characters.");

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\@\!\?\*\.]").WithMessage("Password must contain at least one special character (e.g., @, !, ?, *, .).");
        }
        
    }
}