using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Packages.CQRS.Requests.Queries;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.Application.Features.Packages.Dtos.Validators;
using Rideshare.Application.Features.Packages.Strategies.Filtering;
using Rideshare.Application.Features.Packages.Strategies.Pagination;
using Rideshare.Application.Features.Packages.Strategies.Sorting;

namespace Rideshare.Application.Features.Packages.CQRS.Handlers.Queries;

public class GetPackagesQueryHandler : IRequestHandler<GetPackagesQuery, BasePaginatedResponse<List<PackageDto>>>
{
  private readonly IMapper _mapper;
  private readonly IUnitOfWork _unitOfWork;

  public GetPackagesQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
  {
    _mapper = mapper;
    _unitOfWork = unitOfWork;
  }

  public async Task<BasePaginatedResponse<List<PackageDto>>> Handle(GetPackagesQuery request, CancellationToken cancellationToken)
  {
    var packages = await _unitOfWork.PackageRepository.GetAll();
    var validator = new GetPackagesDtoValidator();

    var validationResult = await validator.ValidateAsync(request.GetPackagesDto, cancellationToken);
    if (!validationResult.IsValid)
    {
      return BasePaginatedResponse<List<PackageDto>>.FailureWithError(
          "Package retrieval failed due to validation errors.",
        validationResult.Errors.Select(e => e.ErrorMessage).ToList()
      );
    }

    var packageOptions = request.GetPackagesDto;

    #region Sorting
    var sorters = new List<IPackageSortingStrategy>
    {
      new PackageSortingByNameStrategy(),
      new PackageSortingByPriceStrategy(),
      new PackageSortingByCreatedAtStrategy(),
      new PackageSortingByAvailableSeatsStrategy(),
      new PackageSortingByTotalSeatsStrategy(),
      new PackageSortingByStartDateTimeStrategy(),
    };

    foreach (var sorter in sorters)
    {
      packages = sorter.Sort(packages, packageOptions);
    }
    #endregion

    #region Filtering
    var filters = new List<IPackageFilteringStrategy>
    {
      new PackageFilteringByDriverAssignmentStatusStrategy(),
      new PackageFilteringByValidityStrategy()
    };

    foreach (var filter in filters)
    {
      packages = filter.Filter(packages, packageOptions);
    }
    #endregion



    #region Pagination
    var paginators = new List<IPackagePaginationStrategy>
    {
      new PackagePaginationStrategy()
    };
    foreach (var paginator in paginators)
    {
      packages = paginator.Paginate(packages, packageOptions);
    }
    #endregion

    return BasePaginatedResponse<List<PackageDto>>.Success(
      _mapper.Map<List<PackageDto>>(packages),
      "Packages retrieved successfully.",
      request.GetPackagesDto.Page,
      int.Min(request.GetPackagesDto.PageSize, packages.Count)
    );
  }
}
