using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos;

public class GetRiderByIdResponseDto
{
  public ObjectId Id { set; get; }
  public DateTime createdAt { set; get; }
  public DateTime UpdatedAt { set; get; }
  public Guid ApplicationUserId { get; set; }
  public string FirstName { set; get; } = null;
  public string LastName { set; get; } = null;
  public string PhoneNumber { set; get; }
  public string Email { set; get; } = null;
  public string ProfilePicture { set; get; } = null;
}