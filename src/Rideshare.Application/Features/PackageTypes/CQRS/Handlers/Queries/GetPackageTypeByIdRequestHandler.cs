using MediatR;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.PackageTypes.CQRS.Requests.Queries;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Application.Features.PackageTypes.CQRS.Handlers.Commands;

public class GetPackageTypeByIdRequestHandler : IRequestHandler<GetPackageTypeByIdRequest, BaseCommandResponse<PackageTyp>>
{
    private readonly IPackageTypeRepository _packageTypeRepository;
    public GetPackageTypeByIdRequestHandler(IPackageTypeRepository packageTypeRepository)
    {
        _packageTypeRepository = packageTypeRepository;
    }

    public async Task<BaseCommandResponse<PackageTyp>> Handle(GetPackageTypeByIdRequest request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse<PackageTyp>();
        var packageType =  await _packageTypeRepository.Get(request.Id);
        response.Value = packageType;
        if(packageType == null)
        {
            response.Succeeded = false;
            response.Message = "Package Type not found";
            return response;
        }
        response.Succeeded = true;
        return response;
    }
}