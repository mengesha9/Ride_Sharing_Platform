using MediatR;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Riders.CQRS.Requests.Commands;
public class UpdateRiderProfilePictureRequest:IRequest<BaseCommandResponse<Rider>>
{
    public ObjectId riderID { get; set; }
    public IFormFile Image { get; set; }
}