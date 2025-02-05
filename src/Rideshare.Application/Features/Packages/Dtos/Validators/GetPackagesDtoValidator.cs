using FluentValidation;

namespace Rideshare.Application.Features.Packages.Dtos.Validators;

public class GetPackagesDtoValidator : AbstractValidator<GetPackagesDto>
{
  public GetPackagesDtoValidator()
  {
    RuleFor(dto => dto.Page).GreaterThan(0);
    RuleFor(dto => dto.PageSize).GreaterThan(0);
  }
}
