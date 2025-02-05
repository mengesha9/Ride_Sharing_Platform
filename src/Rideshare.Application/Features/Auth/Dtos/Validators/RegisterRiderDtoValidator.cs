using FluentValidation;

namespace Rideshare.Application.Features.Auth.Dtos.Validators
{
    public class RegisterRiderDtoValidator : AbstractValidator<RegisterRiderDto>
    {
        public RegisterRiderDtoValidator()
        {
            RuleFor(dto => dto.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(50).WithMessage("Full name must not exceed 50 characters.")
                .MinimumLength(2).WithMessage("Full name must be at least 2 characters long.");

            RuleFor(dto => dto.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(12).WithMessage("Phone number must not exceed 12 characters.")
                .Matches(@"^(251)(9|7)\d{8}$").WithMessage("Phone number is not valid, Phone Number must be in the format 251 followed by 9 or 7 and then 8 digits.");
                

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
        }
    }
}