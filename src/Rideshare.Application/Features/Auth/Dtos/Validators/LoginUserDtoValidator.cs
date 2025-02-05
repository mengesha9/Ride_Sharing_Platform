using FluentValidation;

namespace Rideshare.Application.Features.Auth.Dtos.Validators
{
  public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
  {
    public LoginUserDtoValidator()
    {
      // At least one of the UserName, Email, or PhoneNumber must be provided
            RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.UserName) ||
                                      !string.IsNullOrWhiteSpace(x.Email) ||
                                      !string.IsNullOrWhiteSpace(x.PhoneNumber))
                           .WithMessage("At least one of UserName, Email, or PhoneNumber must be provided.");

            // Email validation if provided
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                                 .WithMessage("Invalid email format.");
            // Phone number validation if provided
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^(251)(9|7)\d{8}$").WithMessage(" Phone number is not valid, Phone Number must be in the format 251 followed by 9 or 7 and then 8 digits.")
                .MaximumLength(12).WithMessage("Phone number must not exceed 12 characters.");

            // Password must meet complexity requirements
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one digit.")
                .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");
    }
  }
}
