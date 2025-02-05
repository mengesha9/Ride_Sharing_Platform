using MongoDB.Bson;

namespace Rideshare.Application.Common.Response;

public class BaseCommandResponse
{
    public ObjectId Id { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}