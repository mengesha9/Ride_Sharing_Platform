using FluentValidation;
using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos.Validators
{
    public class UpdateRiderDeviceTokenRequestDtoValidator : AbstractValidator<UpdateRiderDeviceTokenRequestDto>
    {
        public UpdateRiderDeviceTokenRequestDtoValidator()
        {
            RuleFor(dto => dto.RiderId)
                .NotEmpty().WithMessage("RiderId must not be empty.")
                .Must(riderId => riderId != ObjectId.Empty).WithMessage("Invalid RiderId.");

            RuleFor(dto => dto.DeviceToken)
                .NotEmpty().WithMessage("DeviceToken must not be empty.");
        }
        
    }
}