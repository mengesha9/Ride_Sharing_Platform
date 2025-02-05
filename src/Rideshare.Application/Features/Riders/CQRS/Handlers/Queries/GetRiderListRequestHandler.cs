using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Application.Features.Riders.Dtos.Validators;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Riders.CQRS.Handlers.Queries
{
    public class GetRiderListRequestHandler : IRequestHandler<GetRiderListRequest, BaseResponse<List<GetRidersListResponseDto>>>
    {
        private readonly IRiderRepository _riderRepository;
        private readonly IMapper _mapper;

        public GetRiderListRequestHandler(IRiderRepository riderRepository, IMapper mapper)
        {
            _riderRepository = riderRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<GetRidersListResponseDto>>> Handle(GetRiderListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<GetRidersListResponseDto>>();
            
            // Validate the request
            var validator = new GetRidersListRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request.GetRidersListRequestDto);
            
            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation Error";
                response.Error = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // Fetch paginated data from the repository
            var riders = await _riderRepository.GetPaginated(
                request.GetRidersListRequestDto.PageNumber,
                request.GetRidersListRequestDto.PageSize
            );
            
            // Map to DTO
            var riderDto = _mapper.Map<List<GetRidersListResponseDto>>(riders);
            
            // Set successful response
            response.IsSuccess = true;
            response.Message = "Riders retrieved successfully.";
            response.Value = riderDto;
            
            return response;
        }
    }
}
