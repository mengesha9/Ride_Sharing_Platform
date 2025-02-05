using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.VehicleTypes.CQRS.Requests;
using Rideshare.Application.Services;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.VehicleTypes.CQRS.Handlers;

public class GetListOfVehiclePriceRequestHandler : IRequestHandler<GetListOfVehiclePriceRequest, BaseResponse<Dictionary<VehicleType, Dictionary<string, double>>>>
{
    private readonly IEnumRepository<VehicleType> _vehicleTypeRepository;
    private readonly PaymentCalculatorService _paymentCalculatorService;
    private readonly IMapper _mapper;
    public GetListOfVehiclePriceRequestHandler(IMapper mapper, IEnumRepository<VehicleType> vehicleTypeRepository, PaymentCalculatorService paymentCalculatorService)
    {
        _vehicleTypeRepository = vehicleTypeRepository;
        _paymentCalculatorService = paymentCalculatorService;
        _mapper = mapper;
    }

    public int GetTotalSeats(VehicleType vehicleType)
    {
        switch (vehicleType)
        {
            case VehicleType.Economy:
                return 4;
            case VehicleType.Minibus:
                return 12;
            case VehicleType.Classic:
                return 4;
            case VehicleType.Minivan:
                return 6;
            case VehicleType.Lada:
                return 4;
            default:
                return 4;
        }

    }
    public Task<BaseResponse<Dictionary<VehicleType, Dictionary<string, double>>>> Handle(GetListOfVehiclePriceRequest request, CancellationToken cancellationToken)
    {


        var response = new BaseResponse<Dictionary<VehicleType, Dictionary<string, double>>>();

        var userLocation = _mapper.Map<Location>(request.GetListOfVehiclePriceDto.UserLocation);
        var destination = _mapper.Map<Location>(request.GetListOfVehiclePriceDto.Destination);
        var vehiclePriceDictionary = new Dictionary<VehicleType, Dictionary<string, double>>();
        var distance = DistanceCalculatorService.CalculateDistance(userLocation, destination);
        foreach (VehicleType vehicleType in Enum.GetValues(typeof(VehicleType)))
        {
            var price = _paymentCalculatorService.CalculateVehiclePrice(distance, vehicleType);
            var totalSeats = GetTotalSeats(vehicleType);
            var dict = new Dictionary<string, double>();
            dict.Add("totalSeat", totalSeats);
            dict.Add("price", price);
            vehiclePriceDictionary.Add(vehicleType, dict);
        }

        response.Value = vehiclePriceDictionary;
        response.IsSuccess = true;
        response.Message = "Vehicle prices are successfully calculated";
        return Task.FromResult(response);
    }

}
