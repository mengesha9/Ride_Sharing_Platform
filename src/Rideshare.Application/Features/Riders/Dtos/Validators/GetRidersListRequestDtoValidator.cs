using FluentValidation;

namespace Rideshare.Application.Features.Riders.Dtos.Validators
{
    public class GetRidersListRequestDtoValidator : AbstractValidator<GetRidersListRequestDto>
    {
        public GetRidersListRequestDtoValidator()
        {
            RuleFor(dto => dto.PageNumber)
                .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");

            RuleFor(dto => dto.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }
}