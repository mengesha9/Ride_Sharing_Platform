using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Packages.CQRS.Requests.Queries;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.Application.Services;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Packages.CQRS.Handlers.Queries
{
    public class GetNearbyPackagesHandler : IRequestHandler<GetNearbyPackagesRequest, BaseResponse<List<GetNearByPackageWithDistanceResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetNearbyPackagesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<List<GetNearByPackageWithDistanceResponseDto>>> Handle(GetNearbyPackagesRequest request, CancellationToken cancellationToken)
        {

            var userLocation = _mapper.Map<Location>(request.GetNearbyPackagesDto?.UserLocation);
            var destination = request.GetNearbyPackagesDto?.Destination != null
                      ? _mapper.Map<Location>(request.GetNearbyPackagesDto.Destination)
                      : null;


            var maxRadius = 2;

            var allPackages = await _unitOfWork.PackageRepository.GetAll();

            // Filter packages in memory
            var nearByPackages = allPackages
                .Where(package =>
                    DistanceCalculatorService.CalculateDistance(package.PickUpLocation, userLocation) <= maxRadius &&
                    !package.IsActive && package.IsValid && package.AvailableSeats > 0)
                .OrderBy(package => DistanceCalculatorService.CalculateDistance(package.PickUpLocation, userLocation))
                .ToList();

            // If a destination is provided, further filter by drop-off location
            if (destination != null)
            {
                nearByPackages = nearByPackages
                 .Where(package => DistanceCalculatorService.CalculateDistance(package.DropOffLocation, destination) <= maxRadius).ToList();
            }
            else
            {
                nearByPackages = nearByPackages.ToList();
            }

            // Prepare the response
            var response = new BaseResponse<List<GetNearByPackageWithDistanceResponseDto>>();
            var packageDto = _mapper.Map<List<GetNearByPackageWithDistanceResponseDto>>(nearByPackages);

            foreach (var package in packageDto)
            {
                double dDistance = DistanceCalculatorService.CalculateDistance(package.PickUpLocation, userLocation);
                package.Distance = dDistance; // Manually map the distance
            }

            response.IsSuccess = true;
            response.Message = "Packages retrieved successfully.";
            response.Value = packageDto;

            return response;
        }
    }
}