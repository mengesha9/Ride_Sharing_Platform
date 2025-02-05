using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.PackageTypes.CQRS.Requests.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.PackageTypes.CQRS.Handlers.Commands;
public class CreatePackageTypeCommandHandler : IRequestHandler<CreatePackageTypeCommand, BaseCommandResponse<PackageTyp>>
{
    private readonly IPackageTypeRepository _packageTypeRepository;
    private readonly IMapper _mapper;

    public CreatePackageTypeCommandHandler(IPackageTypeRepository packageTypeRepository, IMapper mapper)
    {
        _packageTypeRepository = packageTypeRepository;
        _mapper = mapper;
    }

    public async Task<BaseCommandResponse<PackageTyp>> Handle(CreatePackageTypeCommand request, CancellationToken cancellationToken)
    {
        var packageType = _mapper.Map<PackageTyp>(request);
        var result=await _packageTypeRepository.Add(packageType);
        var response=new BaseCommandResponse<PackageTyp>();
        response.Value=result;
        if (result == null)
        {
            return new BaseCommandResponse<PackageTyp> { Succeeded = false, Message = "Package Type not found" };
        }
        response.Succeeded = true;
        return response;
    }
}