using FluentValidation;
using MongoDB.Bson;

namespace  Rideshare.Application.Features.Riders.Dtos.Validators
{
    public class GetRiderNotifcationsListRequestDtoValidator : AbstractValidator<GetRiderNotifcationsListRequestDto>
    {
        public GetRiderNotifcationsListRequestDtoValidator()
        {
            RuleFor(dto => dto.Id)
                .Must(BeAValidObjectId).WithMessage("Id must be a valid ObjectId")
                .NotEmpty().WithMessage("Id is required.")
                .NotNull().WithMessage("Id is required.");
        }

        private bool BeAValidObjectId(ObjectId id)
        {
            return id != ObjectId.Empty;
        }  
    }
}
