using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests.Commands;

public class CancelPackageCommand : IRequest<BaseResponse<CancelPackageResponse>>
{
  public CancelPackageRequestDto CancelPackageRequestDto { get; set; } = null!;
}
