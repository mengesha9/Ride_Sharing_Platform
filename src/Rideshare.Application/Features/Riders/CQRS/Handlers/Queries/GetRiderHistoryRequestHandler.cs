using AutoMapper;
using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.DriverHistory.Dtos;
using Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
using Rideshare.Application.Features.Riders.Dtos.Validators;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.DriverHistory.CQRS.Handlers
{

    public class GetRiderHistoryRequestHandler : IRequestHandler<GetRiderHistoryRequest, BaseResponse<List<GetRiderHistoryResponseDto>>>
    {
        private readonly IRiderHistoryRepository _riderHistoryRepository;
        private readonly IMapper _mapper;

        public GetRiderHistoryRequestHandler(IRiderHistoryRepository riderHistoryRepository, IMapper mapper)
        {
            _riderHistoryRepository = riderHistoryRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse<List<GetRiderHistoryResponseDto>>> Handle(GetRiderHistoryRequest request, CancellationToken cancellationToken)
        {
            
            var response = new BaseResponse<List<GetRiderHistoryResponseDto>>();
            var validator = new GetRiderHistoryRequestValidator();
            var validationResult = await validator.ValidateAsync(request.GetRiderHistoryRequestDto, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation Error";
                response.Error = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var riderHistory = await _riderHistoryRepository.GetByRiderId(
                (ObjectId)request.GetRiderHistoryRequestDto.RiderId, 
                request.GetRiderHistoryRequestDto.SortField, 
                request.GetRiderHistoryRequestDto.IsAscending, 
                request.GetRiderHistoryRequestDto.MinPrice, 
                request.GetRiderHistoryRequestDto.MaxPrice, 
                request.GetRiderHistoryRequestDto.StartDate, 
                request.GetRiderHistoryRequestDto.EndDate
            );

            var riderHistoryResponse = _mapper.Map<List<GetRiderHistoryResponseDto>>(riderHistory);
            response.IsSuccess = true;
            response.Message = "Rider History retrieved successfully.";
            response.Value = riderHistoryResponse;

            return response;
        }

    }
}