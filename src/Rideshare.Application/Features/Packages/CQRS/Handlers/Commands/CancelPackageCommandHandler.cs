using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using AutoMapper;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.Application.Features.Packages.CQRS.Requests.Commands;
using Rideshare.Domain.Entities;
using MongoDB.Bson;
using Rideshare.Application.Common.Exceptions;

namespace Rideshare.Application.Features.Packages.CQRS.Handlers.Queries;

public class CancelPackageCommandHandler : IRequestHandler<CancelPackageCommand, BaseResponse<CancelPackageResponse>>
{
    private readonly IPackageRepository _packageRepository;
    private readonly IMapper _mapper;

    public CancelPackageCommandHandler(IMapper mapper, IPackageRepository packageRepository)
    {
        _packageRepository = packageRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<CancelPackageResponse>> Handle(CancelPackageCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<CancelPackageResponse>();

        var package = await _packageRepository.Get(request.CancelPackageRequestDto.PackageId);
        if (package == null)
        {
            throw new NotFoundException("Package not found");
        }
        if (package.IsActive)
        {
            throw new ActivePackageException("User can't edit an active package");
        }
        if (package.IsActive)
        {
            response.IsSuccess = false;
            response.Message = "User can't edit an active package";
            return response;
        }
        var riderId = request.CancelPackageRequestDto.RiderId;
        if (package.RegisteredRiders.Count == 1)
        {
            package.RegisteredRiders.Clear();
            var deletePackageRespone = _mapper.Map<CancelPackageResponse>(package);
            await _packageRepository.Delete(package);
            response.IsSuccess = true;
            response.Value = deletePackageRespone;
            response.Message = "Package cancelled successfully";
        }

        var riderToRemove = package.RegisteredRiders.FirstOrDefault(r => r.Id == riderId);
        if (riderToRemove != null)
        {
            package.RegisteredRiders.Remove(riderToRemove);
            await _packageRepository.Update(package);
            var cancelPackageResponse = _mapper.Map<CancelPackageResponse>(package);
            response.IsSuccess = true;
            response.Value = cancelPackageResponse;
            response.Message = "Package cancelled successfully";
        }
        else
        {
            throw new NotFoundException("Rider not found in package", riderId.ToString());
        }

        return response;
    }
}