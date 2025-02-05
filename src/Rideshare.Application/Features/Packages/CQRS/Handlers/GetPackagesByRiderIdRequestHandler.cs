using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using AutoMapper;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Handlers;

public class GetPackagesByRiderIdRequestHandler : IRequestHandler<GetPackagesByRiderIdRequest, BaseResponse<List<GetPackagesByRiderIdResponseDto>>>
{
    private readonly IRiderHistoryRepository _riderHistoryRepository;
    private readonly IPackageRepository _packageRepository;
    private readonly IMapper _mapper;
    public GetPackagesByRiderIdRequestHandler(IRiderHistoryRepository riderHistoryRepository, IMapper mapper, IPackageRepository packageRepository)
    {
        _packageRepository = packageRepository;
        _riderHistoryRepository = riderHistoryRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<GetPackagesByRiderIdResponseDto>>> Handle(GetPackagesByRiderIdRequest request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<List<GetPackagesByRiderIdResponseDto>>();
        var packages = await _packageRepository.GetAll();
        var filteredPackages = packages.Where(p => p.RegisteredRiders.Any(r => r.Id == request.GetPackagesByRiderIdRequestDto.RiderId)).ToList();
        var packageDto = _mapper.Map<List<GetPackagesByRiderIdResponseDto>>(filteredPackages);
        response.IsSuccess = true;
        response.Message = "Request ";
        response.Value = packageDto;
        return response;
    }
}


