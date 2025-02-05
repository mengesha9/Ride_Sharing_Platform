using FluentValidation;
using Rideshare.Application.Features.Riders.Dtos;
namespace Rideshare.Application.Features.Riders.Dtos.Validators;
public class UpdateRiderProfileDtoValidator:AbstractValidator<UpdateRiderDto>
{
    public UpdateRiderProfileDtoValidator()
    {
        RuleFor(x => x.FirstName)
                .MaximumLength(50).WithMessage("First Name must not exceed 50 characters.")
                .Matches(@"^[^\s]+$").WithMessage("First Name cannot contain spaces.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));
                

            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("Last Name must not exceed 50 characters.")
                .Matches(@"^[^\s]+$").WithMessage("Last Name cannot contain spaces.")
                .When(x => !string.IsNullOrEmpty(x.LastName));
                

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(12).WithMessage("Phone Number must be less than 12 digits.")
                .Matches(@"^(251)(9|7)\d{8}$").WithMessage("Phone Number must be in the format 251 followed by 9 or 7 and then 8 digits.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
                

            RuleFor(x => x.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format. Email must contain a valid domain like .com, .org, etc.")
                .When(x => !string.IsNullOrEmpty(x.Email));
                
    }
}