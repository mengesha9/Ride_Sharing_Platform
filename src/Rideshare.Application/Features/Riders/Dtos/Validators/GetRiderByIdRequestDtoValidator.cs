using FluentValidation;
using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos.Validators;

public class GetRiderByIdRequestDtoValidator : AbstractValidator<GetRiderByIdRequestDto>
{
    public GetRiderByIdRequestDtoValidator()
    {
        RuleFor(dto => dto.RiderId)
            .Must(BeAValidObjectId).WithMessage("RiderId must be a valid ObjectId")
            .NotEmpty().WithMessage("RiderId is required.");
    }
    private bool BeAValidObjectId(ObjectId riderId)
    {
        return riderId != ObjectId.Empty;
    }
}