using AutoMapper;
using Rideshare.Application.Contracts.Identity.Models;
using Rideshare.Application.Features.Auth.Dtos;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Features.DriverHistory.Dtos;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.Application.Features.RiderLocations.Dtos;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Application.Features.Riders.Dtos.Common;
using Rideshare.Domain.Entities;
using Rideshare.Application.Features.PackageTypes.CQRS.Requests.Commands;
using Rideshare.Application.Features.PackageTypes.Dtos;
using Rideshare.Application.Features.Payment.Dtos;
using Rideshare.Domain.Common;
using Rideshare.Application.Features.Waitlists.Dtos;

namespace Rideshare.Application.Profiles
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {

      CreateMap<Package, GetMatchingPackageResponseDto>().ReverseMap();
      CreateMap<Package, GetPackageResponseDto>().ReverseMap();
      CreateMap<Package, GetUnassignedPackagesResponseDto>().ReverseMap();
      CreateMap<Package, CreatePreferedPackageResponseDto>().ReverseMap();
      CreateMap<Package, PackageDto>().ReverseMap();
      CreateMap<Package, GetPackagesByRiderIdResponseDto>().ReverseMap();
      CreateMap<Package, GetActivePackagesByRiderIdResponseDto>().ReverseMap();
      CreateMap<Package, GetPendingPackagesByIdResponseDto>().ReverseMap();
      CreateMap<Package, CancelPackageResponse>().ReverseMap();

      CreateMap<Rider, GetRidersListResponseDto>().ReverseMap();
      CreateMap<Rider, GetRiderByIdResponseDto>().ReverseMap();
      CreateMap<Rider, GetRiderProfileResponseDto>().ReverseMap();

      // waitlist
      CreateMap<Waitlist, WaitlistRequestDto>().ReverseMap();

      CreateMap<RiderHistory, GetPackagesByRiderIdResponseDto>().ReverseMap();
      CreateMap<RiderLocation, GetRiderLocationsResponseDto>().ReverseMap();

      CreateMap<Driver, DriverDto>().ReverseMap();
      CreateMap<Driver, UpdateDriverInformationDto>().ReverseMap();
      CreateMap<Driver, UpdateDriverInformationResponseDto>().ReverseMap();
      CreateMap<Driver, GetDriversResponseDto>().ReverseMap();
      CreateMap<Driver, SearchDriversResponseDto>().ReverseMap();
      CreateMap<RegisterUserRequest, RegisterDriverDto>().ReverseMap();

      CreateMap<RiderNotification, GetRiderNotifcationsListResponseDto>().ReverseMap();


      CreateMap<Package, PreferredPackagesResponseDto>().ReverseMap();
      CreateMap<Package, GetDriverPackagesResponseDto>().ReverseMap();
      CreateMap<Package, GetNearByPackageWithDistanceResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.PackageType, opt => opt.MapFrom(src => src.PackageType.ToString()))
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType.ToString()))
            .ForMember(dest => dest.PickUpLocation, opt => opt.MapFrom(src => new LocationDto(src.PickUpLocation.Latitude, src.PickUpLocation.Longitude, src.PickUpLocation.Name, src.PickUpLocation.PlaceId)))
            .ForMember(dest => dest.DropOffLocation, opt => opt.MapFrom(src => new LocationDto(src.DropOffLocation.Latitude, src.DropOffLocation.Longitude, src.DropOffLocation.Name, src.DropOffLocation.PlaceId)))
            .ForMember(dest => dest.StartDateTime, opt => opt.MapFrom(src => src.StartDateTime))
            .ForMember(dest => dest.TotalSeats, opt => opt.MapFrom(src => src.TotalSeats))
            .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.AvailableSeats))
            .ForMember(dest => dest.Distance, opt => opt.Ignore());




      CreateMap<Location, LocationDto>().ReverseMap();
      CreateMap<Location, LocationRequestDto>().ReverseMap();

      // Driver History
      CreateMap<DriverHistory, GetDriverHIstoryAndEarningsResponseDto>().ReverseMap();

      // Rider History
      CreateMap<RiderHistoryWithPackage, GetRiderHistoryResponseDto>().ReverseMap();
      CreateMap<RefreshTokenResponse, RefreshTokenResponse>().ReverseMap();

      CreateMap<RiderLocation, GetRiderLocationsResponseDto>().ReverseMap();

      CreateMap<Payment, GetTransactionHistoryResponseDto>().ReverseMap();

      #region dto to auth request mappings
      CreateMap<LoginUserDto, LoginUserRequest>().AddTransform<string>(s => string.IsNullOrEmpty(s) ? null : s); ;
      CreateMap<LoginRiderDto, LoginUserRequest>().AddTransform<string>(s => string.IsNullOrEmpty(s) ? null : s); ;

      CreateMap<LoginDriverDto, LoginUserRequest>().AddTransform<string>(s => string.IsNullOrEmpty(s) ? null : s); ;
      CreateMap<RegisterDriverDto, RegisterUserRequest>();

      CreateMap<RegisterAdminDto, RegisterUserRequest>().AddTransform<string>(s => string.IsNullOrEmpty(s) ? null : s); ;
      CreateMap<RegisterRiderDto, RegisterUserRequest>();

      CreateMap<User, UserDto>().ReverseMap().AddTransform<string>(s => string.IsNullOrEmpty(s) ? null : s); ;
      CreateMap<PackageTyp, CreatePackageTypeCommand>().ReverseMap();
      CreateMap<PackageTyp, UpdatePackageTypeCommand>().ReverseMap();
      CreateMap<PackageTyp, PackageTypeDto>().ReverseMap();
      #endregion
    }
  }
}
