using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Handlers;
public class GetMatchingPackageRequestHandler : IRequestHandler<GetMatchingPackageRequest, BaseResponse<List<GetMatchingPackageResponseDto>>>
{
    private readonly IPackageRepository _packageRepository;
    private readonly IMapper _mapper;
    public GetMatchingPackageRequestHandler(IPackageRepository packageRepository, IMapper mapper)
    {
        _packageRepository = packageRepository;
        _mapper = mapper;
    }
    public async Task<BaseResponse<List<GetMatchingPackageResponseDto>>> Handle(GetMatchingPackageRequest request, CancellationToken cancellationToken)
    {
        
        var response = new BaseResponse<List<GetMatchingPackageResponseDto>>();
        var packages = await _packageRepository.GetPackagesByPreference(pickUpLocation: request.GetMatchingPackageRequestDto.PickUpLocation,
                                                            dropOffLocation: request.GetMatchingPackageRequestDto.DropOffLocation,
                                                            startDateTime: request.GetMatchingPackageRequestDto.StartDateTime,
                                                            vehicleType: request.GetMatchingPackageRequestDto.VehicleType,
                                                            packageType: request.GetMatchingPackageRequestDto.PackageType);
        
        var packageDtos = _mapper.Map<List<GetMatchingPackageResponseDto>>(packages);
        response.IsSuccess = true;
        response.Value = packageDtos;
        return response;
    }
}

