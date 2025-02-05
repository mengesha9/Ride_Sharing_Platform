using AutoMapper;
using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.CQRS.Queries;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Features.Drivers.DTOs.validators;

namespace Rideshare.Application.Features.Drivers.CQRS.Handlers
{
  public class GetDriverPackagesQueryHandler : IRequestHandler<GetDriverPackagesQuery, BaseResponse<List<GetDriverPackagesResponseDto>>>
  {
    private readonly IDriverRepository _driverRepository;
    private readonly IMapper _mapper;


    public GetDriverPackagesQueryHandler(IDriverRepository driverRepository, IMapper mapper)
    {
      _driverRepository = driverRepository;
      _mapper = mapper;

    }

    public async Task<BaseResponse<List<GetDriverPackagesResponseDto>>> Handle(GetDriverPackagesQuery request, CancellationToken cancellationToken)
    {
      var response = new BaseResponse<List<GetDriverPackagesResponseDto>>();
      var validator = new GetDriverPackagesQueryValidator(_driverRepository);

      var validationResult = await validator.ValidateAsync(request.getDriverPackagesQueryDto);

      if (validationResult.IsValid == false)
      {
        response.IsSuccess = false;
        response.Message = "Validation failed";
        response.Error = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
      }
      else
      {
        var packages = await _driverRepository.GetDriverPackages(ObjectId.Parse(request.getDriverPackagesQueryDto.DriverId), request.getDriverPackagesQueryDto.PageNumber, request.getDriverPackagesQueryDto.PageSize);
        var packageResponse = _mapper.Map<List<GetDriverPackagesResponseDto>>(packages);
        response.Value = packageResponse;

        response.IsSuccess = true;
      }
      return response;
    }

  }
}
