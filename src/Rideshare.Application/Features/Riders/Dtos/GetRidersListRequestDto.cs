using MongoDB.Bson;

namespace Rideshare.Application.Features.Riders.Dtos;

public class GetRidersListRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10; 
}
