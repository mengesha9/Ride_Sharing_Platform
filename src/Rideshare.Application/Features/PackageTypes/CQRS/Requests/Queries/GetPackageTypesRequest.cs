using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Features.PackageTypes.Dtos;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;



namespace Rideshare.Application.Features.PackageTypes.CQRS.Requests.Queries;

public class GetPackageTypesRequest : IRequest<BaseListResponse<PackageTypeDto>>
{
    
}
