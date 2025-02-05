using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using AutoMapper;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Handlers.Queries;

public class GetPendingPackagesByIdRequestHandler : IRequestHandler<GetPendingPackagesByIdRequest, BaseResponse<List<GetPendingPackagesByIdResponseDto>>>
{
    private readonly IPackageRepository _packageRepository;
    private readonly IMapper _mapper;
    public GetPendingPackagesByIdRequestHandler(IMapper mapper, IPackageRepository packageRepository)
    {
        _packageRepository = packageRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<GetPendingPackagesByIdResponseDto>>> Handle(GetPendingPackagesByIdRequest request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<GetPendingPackagesByIdResponseDto>>();
        var packages = await _packageRepository.GetAll();
        var filteredPackages = packages.Where(p => p.RegisteredRiders.Any(r => r.Id == request.GetPendingPackagesByIdRequestDto.RiderId) && p.AvailableSeats > 0).ToList();
        var packageDto = _mapper.Map<List<GetPendingPackagesByIdResponseDto>>(filteredPackages);
        response.IsSuccess = true;
        response.Message = "Request ";
        response.Value = packageDto;
        return response;
    }
}


