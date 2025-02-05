using AutoMapper;
using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Handlers;

public class CreatePreferedPackageRequestHandler : IRequestHandler<CreatePreferedPackageRequest, BaseResponse<CreatePreferedPackageResponseDto>>
{
    private readonly IPackageRepository _packageRepository;
    private readonly IMapper _mapper;
    private readonly IRiderNotificationRepository _riderNotificationRepository;

    public CreatePreferedPackageRequestHandler(IPackageRepository packageRepository, IMapper mapper)
    {
        _packageRepository = packageRepository;
        _mapper = mapper;
    }
    public async Task<BaseResponse<CreatePreferedPackageResponseDto>> Handle(CreatePreferedPackageRequest request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<CreatePreferedPackageResponseDto>();
        var choosenPackage = await _packageRepository.AddPreferredPackage(request.CreatePreferedPackageRequestDto.DriverId, request.CreatePreferedPackageRequestDto.PackageId);
        if (choosenPackage.AvailableSeats == 0 && choosenPackage.AssignedDriver != ObjectId.Empty)
        {
            await _riderNotificationRepository.SendFullSeatNotification(choosenPackage.RegisteredRiders.Select(r => r.Id).ToList(), choosenPackage);
        }
        var packageDto = _mapper.Map<CreatePreferedPackageResponseDto>(choosenPackage);
        response.Value = packageDto;
        response.IsSuccess = true;
        response.Message = "Package created successfully";
        return response;
    }
}