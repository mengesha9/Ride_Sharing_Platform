using AutoMapper;
using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
using Rideshare.Application.Features.Riders.Dtos.Validators;
using Rideshare.Application.Services;
using Rideshare.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Riders.CQRS.Handlers.Queries
{
    public class JoinPackageRequestHandler : IRequestHandler<JoinPackageRequest, BaseCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public JoinPackageRequestHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(JoinPackageRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            // Validate the request
            var validator = new JoinPackageRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request.JoinPackageRequestDto);

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation Error";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var package = await _unitOfWork.PackageRepository.Get(ObjectId.Parse(request.JoinPackageRequestDto.PackageId));
            if (package == null)
            {
                throw new NotFoundException(nameof(Package), request.JoinPackageRequestDto.PackageId);
            }

            var rider = await _unitOfWork.RiderRepository.Get(request.JoinPackageRequestDto.RiderId);
            if (rider == null)
            {
                throw new NotFoundException("Rider", request.JoinPackageRequestDto.RiderId);
            }

            if (!package.IsValid)
            {
                throw new ConflictException("Package is not valid");
            }

            foreach (var registeredRider in package.RegisteredRiders)
            {
                if (registeredRider.Id == request.JoinPackageRequestDto.RiderId)
                {
                    throw new ConflictException("You have already joined this package");
                }
            }

            if (package.IsActive)
            {
                throw new ConflictException("Package has no available seat");
            }

            package.RegisteredRiders.Add(rider);
            package.AvailableSeats -= 1;

            var riderHistory = new RiderHistory
            {
                Id = ObjectId.GenerateNewId(),
                PackageId = package.Id,
                RiderId = request.JoinPackageRequestDto.RiderId,
                Name = rider.FirstName + " " + rider.LastName,
                PickUpLocation = package.PickUpLocation,
                DropOffLocation = package.DropOffLocation,
                Distance = DistanceCalculatorService.CalculateDistance(package.PickUpLocation, package.DropOffLocation),
                StartDateTime = package.StartDateTime,
                Price = package.Price,
                PackageType = package.PackageType,
                VehicleType = package.VehicleType
            };

            await _unitOfWork.PackageRepository.Update(package);
            await _unitOfWork.RiderHistoryRepository.Add(riderHistory);

            response.Message = "Rider has successfully joined the package";
            response.Id = package.Id;
            response.IsSuccess = true;

            return response;
        }
    }
}
