using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;

namespace Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
public class JoinPackageRequestDto : IRequest<BaseCommandResponse>
{
  public string PackageId { set; get; } = null!;
  public ObjectId RiderId { set; get; }
}