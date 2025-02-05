using AutoMapper;
using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.DriverHistory.Dtos;

namespace Rideshare.Application.Features.DriverHistory.CQRS.Handlers
{

    public class GetDriverHistoryAndEarningsRequestHandler : IRequestHandler<GetDriverHistoryAndEarningsRequest, BaseResponse<List<GetDriverHIstoryAndEarningsResponseDto>>>
    {
        private readonly IDriverHistoryRepository _driverHistoryRepository;
        private readonly IMapper _mapper;

        public GetDriverHistoryAndEarningsRequestHandler(IDriverHistoryRepository driverHistoryRepository, IMapper mapper)
        {
            _driverHistoryRepository = driverHistoryRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse<List<GetDriverHIstoryAndEarningsResponseDto>>> Handle(GetDriverHistoryAndEarningsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<GetDriverHIstoryAndEarningsResponseDto>>();
            var validator = new GetDriverHistoryAndEarningsRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request.GetDriverHIstoryAndEarningsRequestDto);
            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation Error";
                response.Error = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }
            int pageSize = 10;
            int pageNumber = request.GetDriverHIstoryAndEarningsRequestDto.PageNumber;
            int skip = (pageNumber - 1) * pageSize;
            int limit = pageSize;

            var driverHistory = await _driverHistoryRepository.GetByDriverId(ObjectId.Parse(request.GetDriverHIstoryAndEarningsRequestDto.DriverId), skip, limit);
            var driverHistoryResponse = _mapper.Map<List<GetDriverHIstoryAndEarningsResponseDto>>(driverHistory);
            response.IsSuccess = true;
            response.Message = "Driver History and Earnings retrieved successfully.";
            response.Value = driverHistoryResponse;
            return response;
        }
    }
}