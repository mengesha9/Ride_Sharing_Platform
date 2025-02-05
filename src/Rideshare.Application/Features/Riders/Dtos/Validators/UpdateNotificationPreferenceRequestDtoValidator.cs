using FluentValidation;
using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos.Validators
{
    public class UpdateNotificationPreferenceRequestDtoValidator : AbstractValidator<UpdateNotificationPreferenceRequestDto>
    {
        public UpdateNotificationPreferenceRequestDtoValidator()
        {
            RuleFor(dto => dto.RiderId)
                .NotEmpty().WithMessage("RiderId must not be empty.")
                .Must(riderId => riderId != ObjectId.Empty).WithMessage("Invalid RiderId.");
        }
        
    }
}