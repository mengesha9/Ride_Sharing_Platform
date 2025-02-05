using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests;

public class CreatePackageRequest : IRequest<BaseCommandResponse>
{
    public CreatePackageRequestDto CreatePackageRequestDto { set; get; }
    public ObjectId RiderId { set; get; }
}
