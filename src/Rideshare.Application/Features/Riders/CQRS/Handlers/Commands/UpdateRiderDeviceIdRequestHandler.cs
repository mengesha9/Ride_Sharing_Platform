using FluentValidation;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Riders.Dtos.Validators;
using Rideshare.Application.Features.Riders.Requests.Commands;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.CQRS.Handlers.Commands
{
    public class UpdateRiderDeviceIdRequestHandler : IRequestHandler<UpdateRiderDeviceTokenRequest, BaseCommandResponse>
    {
        private readonly IRiderRepository _riderRepository;

        public UpdateRiderDeviceIdRequestHandler(IRiderRepository riderRepository)
        {
            _riderRepository = riderRepository;
        }

        public async Task<BaseCommandResponse> Handle(UpdateRiderDeviceTokenRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            // Validate the request
            var validator = new UpdateRiderDeviceTokenRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UpdateRiderDeviceTokenRequestDto);

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation Error";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var rider = await _riderRepository.Get(request.UpdateRiderDeviceTokenRequestDto.RiderId);

            if (rider == null)
            {
                response.IsSuccess = false;
                response.Message = "Rider not found";
                return response;
            }

            rider.DeviceToken = request.UpdateRiderDeviceTokenRequestDto.DeviceToken;

            // Assuming an update method exists on the repository
            await _riderRepository.Update(rider);

            response.IsSuccess = true;
            response.Id = rider.Id;
            response.Message = "DeviceToken has been updated.";
            return response;
        }
    }
}
