using FluentValidation;
using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos.Validators
{
    public class GetRiderProfileRequestDtoValidator : AbstractValidator<GetRiderProfileRequestDto>
    {
        public GetRiderProfileRequestDtoValidator()
        {
            RuleFor(dto => dto.Id)
                .NotEmpty().WithMessage("Rider Id must not be empty.")
                .Must(id => id != ObjectId.Empty).WithMessage("Invalid Rider Id.");
        }
    }
}