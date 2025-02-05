using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Riders.Requests.Commands;

public class UpdateRiderProfileRequest : IRequest<BaseCommandResponse<Rider>>
{
    public ObjectId riderID {get; set;} 
    public string FirstName { set; get; }
    public string LastName { set; get; }
    public string PhoneNumber;
    public string Email { set; get; }
}
