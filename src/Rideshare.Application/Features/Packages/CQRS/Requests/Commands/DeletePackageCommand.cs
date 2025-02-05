using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests.Commands;

public class DeletePackageCommand : IRequest<BaseResponse<Unit>>
{
  public DeletePackageDto DeletePackageDto { get; set; } = null!;
}
