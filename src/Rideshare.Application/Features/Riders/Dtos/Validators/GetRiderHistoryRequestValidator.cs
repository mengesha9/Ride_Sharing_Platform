using FluentValidation;
using MongoDB.Bson;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Application.Features.Riders.Dtos.Validators
{
    public class GetRiderHistoryRequestValidator : AbstractValidator<GetRiderHistoryRequestDto>
    {
        public GetRiderHistoryRequestValidator()
        {

            RuleFor(x => x.RiderId)
                .NotNull().WithMessage("Rider ID is required.")
                .When(x => x.RiderId.HasValue);

            RuleFor(x => x.SortField)
                .IsInEnum().WithMessage("Invalid Sort Field.");

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Min Price must be greater than or equal to 0.");

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start Date must be less than or equal to End Date.")
                .When(x => x.EndDate != DateOnly.MaxValue);

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End Date must be greater than or equal to Start Date.")
                .When(x => x.StartDate != DateOnly.MinValue);
        }
    }
}