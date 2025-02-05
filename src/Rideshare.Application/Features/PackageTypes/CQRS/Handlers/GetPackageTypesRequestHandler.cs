using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.PackageTypes.CQRS.Requests;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.PackageTypes.CQRS.Handlers;

public class GetPackageTypesRequestHandler : IRequestHandler<GetPackageTypesRequest, BaseResponse<Dictionary<PackageType, int>>>
{
  private readonly IEnumRepository<PackageType> _packageTypeRepository;
  public GetPackageTypesRequestHandler(IEnumRepository<PackageType> packageTypeRepository)
  {
    _packageTypeRepository = packageTypeRepository;
  }

  public Task<BaseResponse<Dictionary<PackageType, int>>> Handle(GetPackageTypesRequest request, CancellationToken cancellationToken)
  {
    var response = new BaseResponse<Dictionary<PackageType, int>>();
    var packageTypeMappings = _packageTypeRepository.GetAllTypeMappings();
    response.Value = packageTypeMappings;
    response.IsSuccess = true;
    return Task.FromResult(response);
  }
}
