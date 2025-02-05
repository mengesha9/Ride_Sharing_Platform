using MediatR;
using Microsoft.AspNetCore.Http;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Application.Features.Riders.CQRS.Requests.Commands;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rideshare.Application.Features.Riders.Dtos.Validators;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Riders.CQRS.Handlers
{
    public class UpdateRiderProfilePictureRequestHandler : IRequestHandler<UpdateRiderProfilePictureRequest, BaseCommandResponse<Rider>>
    {
        private readonly IRiderRepository _riderRepository;
        private readonly IImageUploadService _imageUploadService;

        public UpdateRiderProfilePictureRequestHandler(IRiderRepository riderRepository, IImageUploadService imageUploadService)
        {
            _riderRepository = riderRepository;
            _imageUploadService = imageUploadService;
        }

        public async Task<BaseCommandResponse<Rider>> Handle(UpdateRiderProfilePictureRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse<Rider>();

            // Fetch existing rider profile
            var existingProfile = await _riderRepository.Get(request.riderID);

            if (existingProfile != null)
            {
                // Delete the previous image if exists
                var previousUrl = existingProfile.ProfilePicture;
                if (previousUrl != null)
                {
                    await _imageUploadService.DeleteImage(previousUrl);
                }
            }

            // Validate the image
            var dto = new UpdateRiderProfilePictureDto { Image = request.Image };
            var validator = new UpdateRiderProfilePictureDtoValidator();
            var validationResult = await validator.ValidateAsync(dto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Succeeded = false;
                response.Message = "Validation failed";
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                return response;
            }

            // Upload new image
            var imageUrl = await _imageUploadService.UploadImage(request.Image);

            // Update rider profile with new image URL
            var result = await _riderRepository.UpdateRiderProfilePicture(imageUrl, request.riderID);

            if (result == null)
            {
                response.Succeeded = false;
                response.Message = "Profile picture update failed";
                return response;
            }

            response.Succeeded = true;
            response.Message = "Profile picture updated successfully";

            return response;
        }
    }
}
