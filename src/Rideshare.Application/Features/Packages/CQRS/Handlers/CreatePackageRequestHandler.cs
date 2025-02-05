using System.Runtime.InteropServices;
using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Services;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Packages.CQRS.Handlers;

public class CreatePackageRequestHandler : IRequestHandler<CreatePackageRequest, BaseCommandResponse>
{
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePackageRequestHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<BaseCommandResponse> Handle(CreatePackageRequest request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var rider = await _unitOfWork.RiderRepository.Get(request.RiderId);
        // response.IsSuccess = false;
        // double maxDistance = 2;
        // var unfilteredPackages = await _unitOfWork.PackageRepository.FindAll(package =>
        //     package.VehicleType == request.CreatePackageRequestDto.VehicleType
        //     && package.StartDateTime >= request.CreatePackageRequestDto.StartDateTime.AddMinutes(-15)
        //     && package.StartDateTime <= request.CreatePackageRequestDto.StartDateTime.AddMinutes(15)
        // );

        // var packages = unfilteredPackages
        //     .Where(package =>
        //         DistanceCalculatorService.CalculateDistanceInRadians(package.PickUpLocation, request.CreatePackageRequestDto.PickUpLocation) <= maxDistance
        //         && DistanceCalculatorService.CalculateDistanceInRadians(package.DropOffLocation, request.CreatePackageRequestDto.DropOffLocation) <= maxDistance
        //     ).ToList();


        // if (packages.Count > 0)
        // {
        //     var choosenPackage = packages[0];
        //     packages = packages.OrderBy(package => package.createdAt).ToList();
        //     if (choosenPackage.RegisteredRiders.Any(registeredRider => registeredRider.Id == rider.Id))
        //     {
        //         response.Message = "You are already registered.";
        //         response.IsSuccess = false;
        //         return response;
        //     }
        //     choosenPackage.AvailableSeats -= 1;
        //     choosenPackage.RegisteredRiders.Add(rider);
        //     await _unitOfWork.PackageRepository.Update(choosenPackage);
        //     response.Message = $"{choosenPackage.TotalSeats - choosenPackage.AvailableSeats} people have joined the ride including you.";
        //     var deviceTokens = choosenPackage.RegisteredRiders.Select(r => r.DeviceToken);
        //     foreach (var devicetoken in deviceTokens)
        //     {
        //         if (!String.IsNullOrEmpty(devicetoken))
        //         {
        //             var currRider = await _unitOfWork.RiderRepository.Get(x => x.DeviceToken == devicetoken);
        //             var currRiderId = currRider?.Id;
        //             var riderHistory = await _unitOfWork.RiderHistoryRepository.Get(x => x.PackageId == choosenPackage.Id);
        //             var riderPacakgeName = riderHistory?.Name;
        //             await _notificationService.SendNotificationAsync(devicetoken, $"Additional person joined your {riderPacakgeName} package", $"{choosenPackage.TotalSeats - choosenPackage.AvailableSeats} people have joined the ride including you.", new Dictionary<string, string>());
        //         }
        //     }
        //     if (choosenPackage.AvailableSeats == 0 && choosenPackage.AssignedDriver != ObjectId.Empty)
        //     {
        //         await _unitOfWork.RiderNotificationRepository.SendFullSeatNotification(choosenPackage.RegisteredRiders.Select(r => r.Id).ToList(), choosenPackage);
        //     }
        //     // await _notificationService.SendNotificationAsync("corcwXCKQ-yOj-v07-KxrA:APA91bEJq-NmcoadakV9Nj-PFBkfe_XuTtBhhctMWlkup3qgXZSAljSqw7F3NhMeeFCd3JpCXufmymABcaLGu9V7CRb5GPSY5eAlwmxJ8K4IdlmL0eW4Pkc7UB75pegqaJHvAhJsNe94", "Check title", "check message", new Dictionary<string, string>());
        //     Console.WriteLine("notification sent");
        //     response.Id = choosenPackage.Id;
        // }

        // if (request.PackageId != ObjectId.Empty)
        // {
        //     var package = await _unitOfWork.PackageRepository.Get(request.PackageId);
        //     //check if the rider is not already registerd
        //     if (package.RegisteredRiders.Any(registeredRider => registeredRider.Id == rider.Id))
        //     {
        //         response.Message = "You are already registered .";
        //         return response;
        //     }
        //     package.RegisteredRiders.Add(rider);
        //     package.AvailableSeats -= 1;
        //     await _unitOfWork.PackageRepository.Update(package);
        //     response.Message = $"{package.TotalSeats - package.AvailableSeats} people have joined the ride including you.";
        //     var deviceTokens = package.RegisteredRiders.Select(r => r.DeviceToken);
        //     foreach (var devicetoken in deviceTokens)
        //     {
        //         if (!String.IsNullOrEmpty(devicetoken))
        //         {
        //             var currRider = await _unitOfWork.RiderRepository.Get(x => x.DeviceToken == devicetoken);
        //             var currRiderId = currRider?.Id;
        //             var riderHistory = await _unitOfWork.RiderHistoryRepository.Get(x => x.PackageId == package.Id);
        //             var riderPacakgeName = riderHistory?.Name;
        //             await _notificationService.SendNotificationAsync(devicetoken, $"Additional person joined your {riderPacakgeName} package", $"{package.TotalSeats - package.AvailableSeats} people have joined the ride including you.", new Dictionary<string, string>());
        //         }
        //     }
        //     if (package.AvailableSeats == 0 && package.AssignedDriver != ObjectId.Empty)
        //     {
        //         await _unitOfWork.RiderNotificationRepository.SendFullSeatNotification(package.RegisteredRiders.Select(r => r.Id).ToList(), package);
        //     }
        //     return new BaseCommandResponse { IsSuccess = true, Message = "Package created successfully" };
        // }

        var packageId = ObjectId.GenerateNewId();
        var package = new Package
        {
            Id = packageId,
            Name = request.CreatePackageRequestDto.Name,
            Price = 0,
            PackageType = request.CreatePackageRequestDto.PackageType,
            PickUpLocation = request.CreatePackageRequestDto.PickUpLocation,
            DropOffLocation = request.CreatePackageRequestDto.DropOffLocation,
            StartDateTime = request.CreatePackageRequestDto.StartDateTime,
            TotalSeats = GetTotalSeats(request.CreatePackageRequestDto.VehicleType),
            AvailableSeats = GetTotalSeats(request.CreatePackageRequestDto.VehicleType) - 1,
            VehicleType = request.CreatePackageRequestDto.VehicleType,
            IsValid = true,
            IsActive = false
        };


        var riderHistory = new RiderHistory
        {
            Id = ObjectId.GenerateNewId(),
            PackageId = packageId,
            RiderId = request.RiderId,
            Name = "",
            PickUpLocation = request.CreatePackageRequestDto.PickUpLocation,
            DropOffLocation = request.CreatePackageRequestDto.DropOffLocation,
            Distance = DistanceCalculatorService.CalculateDistance(request.CreatePackageRequestDto.PickUpLocation, request.CreatePackageRequestDto.DropOffLocation),
            StartDateTime = request.CreatePackageRequestDto.StartDateTime,
            Price = 0,
            PackageType = request.CreatePackageRequestDto.PackageType,
            VehicleType = request.CreatePackageRequestDto.VehicleType
        };

        package.RegisteredRiders.Add(rider);
        await _unitOfWork.PackageRepository.Add(package);
        await _unitOfWork.RiderHistoryRepository.Add(riderHistory);
        response.Message = $"Waiting for {package.TotalSeats - 1} more people to join the ride.";
        response.Id = packageId;
        response.IsSuccess = true;
        return new BaseCommandResponse { IsSuccess = true, Message = "Package created successfully" };
    }
    public int GetTotalSeats(VehicleType vehicleType)
    {
        switch (vehicleType)
        {
            case VehicleType.Economy:
                return 4;
            case VehicleType.Minibus:
                return 12;
            case VehicleType.Classic:
                return 4;
            case VehicleType.Minivan:
                return 6;
            case VehicleType.Lada:
                return 4;
            default:
                return 4;
        }

    }

}

