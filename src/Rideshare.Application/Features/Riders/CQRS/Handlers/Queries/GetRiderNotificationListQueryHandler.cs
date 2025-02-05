using AutoMapper;
using MediatR;
using MongoDB.Bson;
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
    public class GetRiderNotificationListQueryHandler : IRequestHandler<GetRiderNotificationListQuery, BaseResponse<List<GetRiderNotifcationsListResponseDto>>>
    {
        private readonly IRiderNotificationRepository _riderNotificationRepository;
        private readonly IMapper _mapper;

        public GetRiderNotificationListQueryHandler(IRiderNotificationRepository riderNotificationRepository, IMapper mapper)
        {
            _riderNotificationRepository = riderNotificationRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<GetRiderNotifcationsListResponseDto>>> Handle(GetRiderNotificationListQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<GetRiderNotifcationsListResponseDto>>();

            // Double check if Id is empty (even though the controller should prevent this)
            if (request.GetRiderNotifcationsListRequestDto.Id == ObjectId.Empty)
            {
                response.IsSuccess = false;
                response.Message = "Unauthorized: Invalid Rider Id.";
                response.Error = new List<string> { "RiderId is not valid." };
                return response;
            }

            // Create and invoke the validator directly
            var validator = new GetRiderNotifcationsListRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request.GetRiderNotifcationsListRequestDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation Error";
                response.Error = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            // Fetch the rider notifications based on the validated Id
            var riderNotifications = await _riderNotificationRepository.GetRiderNotifications(request.GetRiderNotifcationsListRequestDto.Id);
            var riderNotificationsResponse = _mapper.Map<List<GetRiderNotifcationsListResponseDto>>(riderNotifications);

            response.IsSuccess = true;
            response.Message = "Rider notifications retrieved successfully.";
            response.Value = riderNotificationsResponse;

            return response;
        }

    }
}
