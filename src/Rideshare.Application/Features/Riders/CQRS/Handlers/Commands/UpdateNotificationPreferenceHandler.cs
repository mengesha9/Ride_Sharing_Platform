using FluentValidation;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Riders.Dtos.Validators;
using Rideshare.Application.Features.Riders.Requests.Commands;
using Rideshare.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.CQRS.Handlers.Commands
{
    public class UpdateEmailNotificationPreferenceHandler : IRequestHandler<UpdateNotificationPreferenceRequest, BaseCommandResponse>
    {
        private readonly IEmailNotificationRepository _emailNotificationRepository;

        public UpdateEmailNotificationPreferenceHandler(IEmailNotificationRepository emailNotificationRepository)
        {
            _emailNotificationRepository = emailNotificationRepository;
        }

        public async Task<BaseCommandResponse> Handle(UpdateNotificationPreferenceRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            // Validate the request
            var validator = new UpdateNotificationPreferenceRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UpdateNotificationPreferenceRequestDto);

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation Error";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var emailNotification = await _emailNotificationRepository.Get(request.UpdateNotificationPreferenceRequestDto.RiderId);

            if (emailNotification != null) // If email notification exists, update the preference
            {
                emailNotification.IsEnabled = !emailNotification.IsEnabled;
                await _emailNotificationRepository.Update(emailNotification);
            }
            else
            {
                emailNotification = new EmailNotification
                {
                    RiderId = request.UpdateNotificationPreferenceRequestDto.RiderId,
                    IsEnabled = true
                };
                await _emailNotificationRepository.Add(emailNotification);
            }

            response.IsSuccess = true;
            response.Message = "Email notification preference has been updated.";
            return response;
        }
    }
}
