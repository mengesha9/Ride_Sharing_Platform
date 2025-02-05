using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Packages.CQRS.Requests.Commands;

namespace Rideshare.Application.Features.Packages.CQRS.Handlers.Commands;

public class DeletePackageCommandHandler : IRequestHandler<DeletePackageCommand, BaseResponse<Unit>>
{
  private readonly IMapper _mapper;
  private readonly IUnitOfWork _unitOfWork;

  public DeletePackageCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
  {
    _mapper = mapper;
    _unitOfWork = unitOfWork;
  }

  public async Task<BaseResponse<Unit>> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
  {
    var package = await _unitOfWork.PackageRepository.Get(request.DeletePackageDto.PackageId);

    if (package is null)
    {
      return BaseResponse<Unit>.FailureWithError("Package could not be removed because it doesn't exist.", new List<string> { "Package not found." });
    }

    await _unitOfWork.PackageRepository.Delete(package);

    return BaseResponse<Unit>.Success(Unit.Value, "Package removed successfully.");
  }
}
