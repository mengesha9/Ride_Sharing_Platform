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
    public class GetRiderProfileRequestHandler : IRequestHandler<GetRiderProfileRequest, BaseResponse<GetRiderProfileResponseDto>>
    {
        private readonly IRiderRepository _riderRepository;
        private readonly IMapper _mapper;

        public GetRiderProfileRequestHandler(IRiderRepository riderRepository, IMapper mapper)
        {
            _riderRepository = riderRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetRiderProfileResponseDto>> Handle(GetRiderProfileRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetRiderProfileResponseDto>();

            // Validate the request
            var validator = new GetRiderProfileRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request.GetRiderProfileRequestDto);

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation Error";
                response.Error = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // Fetch the rider profile
            var rider = await _riderRepository.Get(request.GetRiderProfileRequestDto.Id);

            if (rider == null)
            {
                response.IsSuccess = false;
                response.Message = "Rider not found.";
                return response;
            }

            // Map to response DTO
            var responseDto = _mapper.Map<GetRiderProfileResponseDto>(rider);
            response.IsSuccess = true;
            response.Message = "Profile fetched successfully.";
            response.Value = responseDto;

            return response;
        }
    }
}
