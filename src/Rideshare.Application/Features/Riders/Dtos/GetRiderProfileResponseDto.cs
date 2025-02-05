using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos;

public class GetRiderProfileResponseDto
{
    public ObjectId Id { get; set; }
    public string FirstName { set; get; }
    public string LastName { set; get; }
    public string PhoneNumber { set; get; }
    public string Email { set; get; } 
    public string ProfilePicture { set; get; }
}
