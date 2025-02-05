using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using MediatR;

namespace Rideshare.Application.Features.PackageTypes.CQRS.Requests.Queries;
public class GetPackageTypeByIdRequest : IRequest<BaseCommandResponse<PackageTyp>>
{
    public int Id ;
}