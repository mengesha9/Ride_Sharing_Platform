using FluentValidation;
using Rideshare.Application.Features.Waitlists.Dtos;

namespace RideShare.Application.Features.Waitlists.Dtos.Validators
{
    

    public class WaitlistRequestDtoValidator : AbstractValidator<WaitlistRequestDto>
    {
        public WaitlistRequestDtoValidator()
        {
            RuleFor(dto => dto.FullName)
                .NotEmpty().WithMessage("FullName is required.")
                .MaximumLength(100).WithMessage("FullName must not exceed 100 characters.");

            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                .EmailAddress().WithMessage("Email is not valid.");

            RuleFor(dto => dto.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.")
                .MaximumLength(12).WithMessage("PhoneNumber must not exceed 12 characters.")
                .Matches(@"^251[79]\d{8}$").WithMessage("PhoneNumber must start with '251', followed by either '9' or '7', and then 8 digits.");
        }
    }
}
