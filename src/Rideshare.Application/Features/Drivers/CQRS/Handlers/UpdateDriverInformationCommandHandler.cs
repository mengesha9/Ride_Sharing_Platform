using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.CQRS.Commands;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Features.Drivers.DTOs.validators;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Drivers.CQRS.Handlers
{
  public class UpdateDriverInformationCommandHandler : IRequestHandler<UpdateDriverInformationCommand, BaseResponse<UpdateDriverInformationResponseDto>>
  {
    private readonly IDriverRepository _driverRepository;
    private readonly IMapper _mapper;
    private readonly IImageUploadService _imageUploadService;

    public UpdateDriverInformationCommandHandler(IDriverRepository driverRepository, IMapper mapper, IImageUploadService imageUploadService)
    {
      _driverRepository = driverRepository;
      _mapper = mapper;
      _imageUploadService = imageUploadService;
    }

    public async Task<BaseResponse<UpdateDriverInformationResponseDto>> Handle(UpdateDriverInformationCommand request, CancellationToken cancellationToken)
    {
      var response = new BaseResponse<UpdateDriverInformationResponseDto>();
      var validator = new UpdateDriverInformationCommandValidator(_driverRepository);

      var validationResult = await validator.ValidateAsync(request.UpdateDriverInformationDto, cancellationToken);

      if (validationResult.IsValid == false)
      {
        response.IsSuccess = false;
        response.Message = "Validation failed";
        response.Error = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
      }

      var driver = new Driver
      {
        FirstName = request.UpdateDriverInformationDto.FirstName,
        LastName = request.UpdateDriverInformationDto.LastName,
        PhoneNumber = request.UpdateDriverInformationDto.PhoneNumber,
        Email = request.UpdateDriverInformationDto.Email,
        Username = request.UpdateDriverInformationDto.Username,
        LicenseNumber = request.UpdateDriverInformationDto.LicenseNumber,
        LicenseExpirationDate = request.UpdateDriverInformationDto.LicenseExpirationDate ?? null,
        LicensePlateNumber = request.UpdateDriverInformationDto.LicensePlateNumber,
        IsVerified = false,
      };

      if (request.UpdateDriverInformationDto.ProfilePicture != null)
      {
        driver.ProfilePicture = await _imageUploadService.UploadImage(request.UpdateDriverInformationDto.ProfilePicture);
      }
      if (request.UpdateDriverInformationDto.DriversLicenseImage != null)
      {
        driver.DriversLicenseImage = await _imageUploadService.UploadImage(request.UpdateDriverInformationDto.DriversLicenseImage);
      }
      if (request.UpdateDriverInformationDto.CarPhotoFront != null)
      {
        driver.CarPhotoFront = await _imageUploadService.UploadImage(request.UpdateDriverInformationDto.CarPhotoFront);
      }
      if (request.UpdateDriverInformationDto.CarPhotoBack != null)
      {
        driver.CarPhotoBack = await _imageUploadService.UploadImage(request.UpdateDriverInformationDto.CarPhotoBack);
      }
      if (request.UpdateDriverInformationDto.CarPhotoLeft != null)
      {
        driver.CarPhotoLeft = await _imageUploadService.UploadImage(request.UpdateDriverInformationDto.CarPhotoLeft);
      }
      if (request.UpdateDriverInformationDto.CarPhotoRight != null)
      {
        driver.CarPhotoRight = await _imageUploadService.UploadImage(request.UpdateDriverInformationDto.CarPhotoRight);
      }
      if (request.UpdateDriverInformationDto.Code3Classification != null)
      {
        driver.Code3Classification = await _imageUploadService.UploadImage(request.UpdateDriverInformationDto.Code3Classification);
      }
      if (request.UpdateDriverInformationDto.TradeLiscenseInformation != null)
      {
        driver.TradeLiscenseInformation = await _imageUploadService.UploadImage(request.UpdateDriverInformationDto.TradeLiscenseInformation);
      }

      var result = await _driverRepository.Add(driver);
      response.IsSuccess = true;
      response.Value = _mapper.Map<UpdateDriverInformationResponseDto>(result);
      return response;
    }
  }
}
