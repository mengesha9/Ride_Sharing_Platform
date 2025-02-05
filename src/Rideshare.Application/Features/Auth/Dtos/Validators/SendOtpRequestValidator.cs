using FluentValidation;

namespace Rideshare.Application.Features.Auth.Dtos.Validators
{
    public class SendOtpRequestValidator : AbstractValidator<SendOtpRequest>
    {
        public SendOtpRequestValidator()
        {
            RuleFor(dto => dto.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^(251)(9|7)\d{8}$").WithMessage("Phone number is not valid, Phone Number must be in the format 251 followed by 9 or 7 and then 8 digits.")
                .MaximumLength(12).WithMessage("Phone number must not exceed 12 characters.");
                
        }
        
    }
}