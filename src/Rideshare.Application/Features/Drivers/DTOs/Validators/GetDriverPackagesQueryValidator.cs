using FluentValidation;
using MongoDB.Bson;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Application.Features.Drivers.DTOs.validators
{
    public class GetDriverPackagesQueryValidator : AbstractValidator<GetDriverPackagesQueryDto>
    {
        public GetDriverPackagesQueryValidator(IDriverRepository driverRepository)
        {
            RuleFor(driver => driver.DriverId)
                .NotEmpty().WithMessage("Driver ID is required.")
                .Must(id => ObjectId.TryParse(id, out _)).WithMessage("Invalid driver ID format.");
            RuleFor(p => p.PageNumber)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
                .MustAsync(async (pageNumber, cancellation) =>
                {
                    var queryDto = new GetDriverPackagesQueryDto { PageNumber = pageNumber, DriverId = "" };
                    return await IsLessThanDBCount(queryDto, driverRepository) || pageNumber == 1;
                });
            RuleFor(p => p.PageSize)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
                .LessThanOrEqualTo(15).WithMessage("{PropertyName} must be less than or equal to 10");
        }

        private static async Task<bool> IsLessThanDBCount(GetDriverPackagesQueryDto queryDto, IDriverRepository driverRepository)
        {
            var pageSize = queryDto.PageSize;
            return (queryDto.PageSize - 1) * pageSize < await driverRepository.Count();
        }
    }
}
