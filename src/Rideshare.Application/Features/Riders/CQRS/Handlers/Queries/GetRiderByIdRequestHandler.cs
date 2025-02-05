using AutoMapper;
using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Riders.CQRS.Requests.Queries;
using Rideshare.Application.Features.Riders.Dtos;
using Rideshare.Application.Features.Riders.Dtos.Validators;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Riders.CQRS.Handlers.Queries
{
    public class GetRiderByIdRequestHandler : IRequestHandler<GetRiderByIdRequest, BaseResponse<GetRiderByIdResponseDto>>
    {
        private readonly IRiderRepository _riderRepository;
        private readonly IMapper _mapper;

        public GetRiderByIdRequestHandler(IRiderRepository riderRepository, IMapper mapper)
        {
            _riderRepository = riderRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<GetRiderByIdResponseDto>> Handle(GetRiderByIdRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<GetRiderByIdResponseDto>();

            // Create and invoke the validator directly
            var validator = new GetRiderByIdRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request.GetRiderByIdRequestDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation Error";
                response.Error = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            // Fetch the rider based on the validated RiderId
            var rider = await _riderRepository.Get(request.GetRiderByIdRequestDto.RiderId);

            if (rider == null)
            {
                response.IsSuccess = false;
                response.Message = "Rider not found.";
                return response;
            }

            // Map the Rider entity to the response DTO
            var riderResponseDto = _mapper.Map<GetRiderByIdResponseDto>(rider);
            response.IsSuccess = true;
            response.Message = "Rider retrieved successfully.";
            response.Value = riderResponseDto;

            return response;
        }
    }
}
