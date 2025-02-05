using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.CQRS.Queries;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Features.Drivers.DTOs.validators;

namespace Rideshare.Application.Features.Drivers.CQRS.Handlers
{
    public class GetDriversQueryHandler : IRequestHandler<GetDriversQuery, BaseResponse<List<GetDriversResponseDto>>>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IMapper _mapper;

        public GetDriversQueryHandler(IDriverRepository driverRepository, IMapper mapper)
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<GetDriversResponseDto>>> Handle(GetDriversQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<GetDriversResponseDto>>();
            var validator = new GetDriversQueryValidator(_driverRepository);
            var validationResult = await validator.ValidateAsync(request.getDriversQueryDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation Error";
                response.Error = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }
            var drivers = await _driverRepository.GetDrivers(request.getDriversQueryDto.pageNumber, request.getDriversQueryDto.pageSize);
            var driversResponse = _mapper.Map<List<GetDriversResponseDto>>(drivers);
            response.IsSuccess = true;
            response.Message = $"{((request.getDriversQueryDto.pageNumber - 1) * request.getDriversQueryDto.pageSize) + 1}-{Math.Min(request.getDriversQueryDto.pageNumber * request.getDriversQueryDto.pageSize, drivers.Count)} of {await _driverRepository.Count()} drivers retrieved.";
            response.Value = driversResponse;
            return response;
        }
    }
}