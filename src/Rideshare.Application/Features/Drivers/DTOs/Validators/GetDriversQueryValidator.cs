using FluentValidation;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Application.Features.Drivers.DTOs.validators
{
  public class GetDriversQueryValidator : AbstractValidator<GetDriversQueryDto>
  {
    public GetDriversQueryValidator(IDriverRepository driverRepository)
    {
      RuleFor(p => p.pageNumber)
          .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
          .MustAsync(async (pageNumber, cancellation) =>
          {
            var queryDto = new GetDriversQueryDto { pageNumber = pageNumber };
            return await IsLessThanDBCount(queryDto, driverRepository) || pageNumber == 1;
          });
      RuleFor(p => p.pageSize)
          .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
          .LessThanOrEqualTo(15).WithMessage("{PropertyName} must be less than or equal to 15");
    }

    private static async Task<bool> IsLessThanDBCount(GetDriversQueryDto queryDto, IDriverRepository driverRepository)
    {
      var pageSize = queryDto.pageSize;
      return (queryDto.pageNumber - 1) * pageSize < await driverRepository.Count();
    }
  }
}
