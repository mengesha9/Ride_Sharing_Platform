using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.RiderLocations.Dtos;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.RiderLocations.CQRS.Requests;

public class UpdateRiderLocationRequest : IRequest<BaseCommandResponse>
{
    public required ObjectId Id { set; get; }
    public ObjectId RiderId { set; get; }
    public required string Name { set; get; }
    public required Location Location { set; get; }}
