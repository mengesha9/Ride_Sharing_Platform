using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;

namespace Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
public class JoinPackageRequest : IRequest<BaseCommandResponse>
{
  public JoinPackageRequestDto JoinPackageRequestDto { set; get; } = null!;
}