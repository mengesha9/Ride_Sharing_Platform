using AutoMapper;
using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Identity.Services;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.CQRS.Commands;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Features.Drivers.DTOs.validators;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Drivers.CQRS.Handlers
{
    public class DeleteDriverCommandHandler : IRequestHandler<DeleteDriverCommand, BaseResponse<DeleteDriverResponseDto>>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IAuthService _authService;

        public DeleteDriverCommandHandler(IDriverRepository driverRepository, IAuthService authService)
        {
            _driverRepository = driverRepository;
            _authService = authService;
        }

        public async Task<BaseResponse<DeleteDriverResponseDto>> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<DeleteDriverResponseDto>();
            var validator = new DeleteDriverCommandValidator();

            var validationResult = await validator.ValidateAsync(request.deleteDriverDto);

            if (validationResult.IsValid == false)
            {
                response.IsSuccess = false;
                response.Message = "Validation failed";
                response.Error = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                Driver driver = await _driverRepository.GetDriverById(ObjectId.Parse(request.deleteDriverDto.driverId));
                bool result = await _driverRepository.DeleteDriver(ObjectId.Parse(request.deleteDriverDto.driverId));
                bool identityResult = await _authService.DeleteUser(driver.ApplicationUserId);
                response.IsSuccess = result && identityResult;
                if (response.IsSuccess)
                {
                    response.Message = "Driver deleted successfully";
                    response.Value = new DeleteDriverResponseDto { deletionSuccessfull = true };
                }
                else
                {
                    response.Message = "Driver deletition failed";
                    response.Value = new DeleteDriverResponseDto { deletionSuccessfull = false };
                }
                
            }
            return response;
        }
    }
}