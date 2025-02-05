using FluentValidation;
using MongoDB.Bson;
using Rideshare.Application.Features.DriverHistory.Dtos;

namespace Rideshare.Application.Features.DriverHistory.Dtos
{
    public class GetDriverHistoryAndEarningsRequestDtoValidator : AbstractValidator<GetDriverHIstoryAndEarningsRequestDto>
    {
        public GetDriverHistoryAndEarningsRequestDtoValidator()
        {
            RuleFor(dto => dto.DriverId)
                .NotEmpty()
                .WithMessage("DriverId is required.");

            RuleFor(dto => dto.DriverId)
                .Must(id => ObjectId.TryParse(id.ToString(), out _))
                .WithMessage("DriverId must be a valid ObjectId format.");
        }
    }
}