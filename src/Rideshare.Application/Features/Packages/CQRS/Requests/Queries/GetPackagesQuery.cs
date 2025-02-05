using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests.Queries;

public class GetPackagesQuery : IRequest<BasePaginatedResponse<List<PackageDto>>>
{
  public GetPackagesDto GetPackagesDto { get; set; } = null!;
}
