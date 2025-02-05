using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.Dtos;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Packages.CQRS.Handlers;

public class GetActivePackagesByRiderIdRequestHander : IRequestHandler<GetActivePackagesByRiderIdRequest, BaseResponse<List<GetActivePackagesByRiderIdResponseDto>>>
{
  private readonly IPackageRepository _packageRepository;
  private readonly IMapper _mapper;

  public GetActivePackagesByRiderIdRequestHander(IPackageRepository packageRepository, IMapper mapper)
  {
    _packageRepository = packageRepository;
    _mapper = mapper;
  }

  public async Task<BaseResponse<List<GetActivePackagesByRiderIdResponseDto>>> Handle(GetActivePackagesByRiderIdRequest request, CancellationToken cancellationToken)
  {
    var response = new BaseResponse<List<GetActivePackagesByRiderIdResponseDto>>();
    var RiderId = request.GetActivePackagesByRiderIdRequestDto.RiderId;
    var packages = await _packageRepository.GetAll();
    var filteredPackages = packages.Where(x => x.RegisteredRiders.Any(r => r.Id == RiderId) && x.IsActive).ToList();
    var currentDate = DateTime.Now;
    var activePackages = filteredPackages.Where(p => p.StartDateTime.AddDays(p.PackageType == PackageType.Weekly ? 7 : p.PackageType == PackageType.Monthly ? 30 : 0) >= currentDate).ToList();

    var packageDto = _mapper.Map<List<GetActivePackagesByRiderIdResponseDto>>(activePackages);
    response.IsSuccess = true;
    response.Message = "Packages retrieved successfully.";
    response.Value = packageDto;
    return response;
  }
}