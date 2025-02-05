using FluentValidation;
using MongoDB.Bson;
using Rideshare.Application.Features.Riders.CQRS.Requests.Queries;

namespace Rideshare.Application.Features.Riders.Dtos.Validators
{
    public class JoinPackageRequestDtoValidator : AbstractValidator<JoinPackageRequestDto>
    {
        public JoinPackageRequestDtoValidator()
        {
            RuleFor(dto => dto.PackageId)
                .NotEmpty().WithMessage("PackageId must not be empty.")
                .Must(packageId => ObjectId.TryParse(packageId, out _)).WithMessage("Invalid PackageId format.");

            RuleFor(dto => dto.RiderId)
                .NotEmpty().WithMessage("RiderId must not be empty.")
                .Must(riderId => riderId != ObjectId.Empty).WithMessage("Invalid RiderId.");
        }
    }
}