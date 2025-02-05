using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Drivers.CQRS.Queries;
using Rideshare.Application.Features.Drivers.DTOs;
using Rideshare.Application.Features.Drivers.DTOs.validators;

namespace Rideshare.Application.Features.Drivers.CQRS.Handlers
{
    public class SearchDriversQueryHandler : IRequestHandler<SearchDriversQuery, BaseResponse<List<SearchDriversResponseDto>>>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IMapper _mapper;

        public SearchDriversQueryHandler(IDriverRepository driverRepository, IMapper mapper)
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<SearchDriversResponseDto>>> Handle(SearchDriversQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<SearchDriversResponseDto>>();
            var validator = new SearchDriversQueryValidator(_driverRepository);
            var validationResult = await validator.ValidateAsync(request.searchDriversQueryDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation Error";
                response.Error = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }
            var drivers = await _driverRepository.SearchDrivers(request.searchDriversQueryDto.SearchTerm, request.searchDriversQueryDto.Page, request.searchDriversQueryDto.Size);
            var driversResponse = _mapper.Map<List<SearchDriversResponseDto>>(drivers);
            response.IsSuccess = true;
            response.Message = $"{((request.searchDriversQueryDto.Page - 1) * request.searchDriversQueryDto.Size) + 1}-{Math.Min(request.searchDriversQueryDto.Page * request.searchDriversQueryDto.Size, drivers.Count)} of {await _driverRepository.Count()} drivers retrieved.";
            response.Value = driversResponse;
            return response;
        }
    }
}