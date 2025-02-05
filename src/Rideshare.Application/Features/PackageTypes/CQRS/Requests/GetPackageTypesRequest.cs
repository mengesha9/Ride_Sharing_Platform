using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Domain.Common;



namespace Rideshare.Application.Features.PackageTypes.CQRS.Requests;

public class GetPackageTypesRequest : IRequest<BaseResponse<Dictionary<PackageType, int>>>
{
    
}
