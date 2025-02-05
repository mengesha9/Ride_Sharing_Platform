using FluentValidation;
using Microsoft.AspNetCore.Http;
using Rideshare.Application.Features.Riders.Dtos;
namespace Rideshare.Application.Features.Riders.Dtos.Validators;
public class UpdateRiderProfilePictureDtoValidator :AbstractValidator<UpdateRiderProfilePictureDto>
{
    private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

        public UpdateRiderProfilePictureDtoValidator()
        {
            RuleFor(x => x.Image)
                .Must(BeValidFile).WithMessage("Invalid image file.")
                .Must(BeAllowedFileType).WithMessage("Invalid file type.")
                .Must(BeAllowedFileSize).WithMessage("File size must not exceed 10 MB.");
        }

        private bool BeValidFile(IFormFile file)
        {
            return file?.Length > 0;
        }

        private bool BeAllowedFileType(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return AllowedExtensions.Contains(extension);
        }

        private bool BeAllowedFileSize(IFormFile file)
        {
            return file.Length <= MaxFileSize;
        }
}