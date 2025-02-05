using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using AutoMapper;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Handlers;

public class GetUnassignedPackagesRequestHandler : IRequestHandler<GetUnassignedPackagesRequest, BaseResponse<List<GetUnassignedPackagesResponseDto>>> 
{
    private readonly IPackageRepository _packageRepository;
    private readonly IMapper _mapper;
    public GetUnassignedPackagesRequestHandler(IPackageRepository packageRepository, IMapper mapper)
    {
        _packageRepository = packageRepository;
        _mapper = mapper;
    }
    public async Task<BaseResponse<List<GetUnassignedPackagesResponseDto>>> Handle(GetUnassignedPackagesRequest request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<GetUnassignedPackagesResponseDto>>();
        var packages = await _packageRepository.GetUnassignedPackages();
        var packageDto = _mapper.Map<List<GetUnassignedPackagesResponseDto>>(packages);
        response.IsSuccess = true;
        response.Message = "Packages retrieved successfully.";
        response.Value = packageDto;
        return response;
    }
}
