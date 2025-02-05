using FluentValidation;
using MongoDB.Bson;

namespace Rideshare.Application.Features.Drivers.DTOs.validators
{
    public class VerifyDriverCommandValidator : AbstractValidator<VerifyDriverDto>
    {
        public VerifyDriverCommandValidator()
        {
            RuleFor(driver => driver.driverId)
                .NotEmpty().WithMessage("Driver ID is required.")
                .Must(id => ObjectId.TryParse(id, out _)).WithMessage("Invalid driver ID format.");

        }
    }
}
