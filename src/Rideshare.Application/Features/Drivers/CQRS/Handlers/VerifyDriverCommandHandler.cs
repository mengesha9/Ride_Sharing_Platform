using AutoMapper;
using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.CQRS.Commands;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Features.Drivers.DTOs.validators;

namespace Rideshare.Application.Features.Drivers.CQRS.Handlers
{
  public class VerifyDriverCommandHandler : IRequestHandler<VerifyDriverCommand, BaseResponse<VerifyDriverResponseDto>>
  {
    private readonly IDriverRepository _driverRepository;

    public VerifyDriverCommandHandler(IDriverRepository driverRepository)
    {
      _driverRepository = driverRepository;
    }

    public async Task<BaseResponse<VerifyDriverResponseDto>> Handle(VerifyDriverCommand request, CancellationToken cancellationToken)
    {
      var response = new BaseResponse<VerifyDriverResponseDto>();
      var validator = new VerifyDriverCommandValidator();

      var validationResult = await validator.ValidateAsync(request.verifyDriverDto);

      if (validationResult.IsValid == false)
      {
        response.IsSuccess = false;
        response.Message = "Validation failed";
        response.Error = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
      }
      else
      {
        bool result = await _driverRepository.VerifyDriver(ObjectId.Parse(request.verifyDriverDto.driverId));
        if (result)
        {
          response.IsSuccess = true;
          response.Message = "Driver verified successfully";
          response.Value = new VerifyDriverResponseDto { verificationSuccessfull = true };
        }
        else
        {
          response.IsSuccess = false;
          response.Message = "Driver verification failed";
          response.Value = new VerifyDriverResponseDto { verificationSuccessfull = false };
        }

      }
      return response;
    }
  }
}
