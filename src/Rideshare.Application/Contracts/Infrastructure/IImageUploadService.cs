using Microsoft.AspNetCore.Http;

namespace Rideshare.Application.Contracts.Infrastructure;

public interface IImageUploadService
{
    Task<string> UploadImage(IFormFile image);
    Task<bool> DeleteImage(string imageUrl);
}