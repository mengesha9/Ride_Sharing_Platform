using Microsoft.AspNetCore.Http;
namespace Rideshare.Application.Features.Riders.Dtos;
public class UpdateRiderProfilePictureDto
{
    public IFormFile Image {set;get;}
}