using FluentValidation;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Application.Features.Riders.Dtos.Validators;
using Rideshare.Application.Features.Riders.Requests.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.CQRS.Handlers.Commands
{
    public class UpdateRiderProfileRequestHandler : IRequestHandler<UpdateRiderProfileRequest, BaseCommandResponse<Rider>>
    {
        private readonly IRiderRepository _riderRepository;

        public UpdateRiderProfileRequestHandler(IRiderRepository riderRepository)
        {
            _riderRepository = riderRepository;
        }

        public async Task<BaseCommandResponse<Rider>> Handle(UpdateRiderProfileRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse<Rider>();

            // Prepare DTO
            var dto = new UpdateRiderDto
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            };

            // Validate the DTO
            var validator = new UpdateRiderProfileDtoValidator();
            var validationResult = await validator.ValidateAsync(dto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Succeeded = false;
                response.Message = "Validation failed";
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                return response;
            }

            // Update rider profile
            var result = await _riderRepository.UpdateRiderProfile(dto, request.riderID);

            if (result == null)
            {
                response.Succeeded = false;
                response.Message = "Rider profile update failed";
                return response;
            }

            response.Succeeded = true;
            response.Message = "Rider profile updated successfully";
            

            return response;
        }
    }
}
