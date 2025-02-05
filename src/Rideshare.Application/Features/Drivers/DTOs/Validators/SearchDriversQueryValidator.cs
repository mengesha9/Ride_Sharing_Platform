using FluentValidation;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Application.Features.Drivers.DTOs.validators
{
    public class SearchDriversQueryValidator : AbstractValidator<SearchDriversQueryDto>
    {
        private IDriverRepository _driverRepository;
        public SearchDriversQueryValidator(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;

            RuleFor(driver => driver.SearchTerm)
                .NotEmpty().WithMessage("Search term is required.")
                .MaximumLength(50).WithMessage("Search term must not exceed 50 characters.");

            RuleFor(p => p.Page)
               .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
               .MustAsync(async (page, cancellation) =>
               {
                   var queryDto = new SearchDriversQueryDto { Page = page };
                   return await IsLessThanDBCount(queryDto, driverRepository) || page == 1;
               });
            RuleFor(p => p.Size)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
                .LessThanOrEqualTo(15).WithMessage("{PropertyName} must be less than or equal to 10");

        }

        private static async Task<bool> IsLessThanDBCount(SearchDriversQueryDto queryDto, IDriverRepository driverRepository)
        {
            var pageSize = queryDto.Size;
            return (queryDto.Page - 1) * pageSize < await driverRepository.Count();
        }
    }
}