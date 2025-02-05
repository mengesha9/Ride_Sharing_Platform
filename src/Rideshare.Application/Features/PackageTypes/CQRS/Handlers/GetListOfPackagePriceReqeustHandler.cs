using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Response;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Services;
using Rideshare.Domain.Common;
using RideShare.Application.Features.PackageTypes.CQRS.Requests.Queries;

namespace RideShare.Application.Features.PackageTypes.CQRS.Handlers.Queries;

public class GetListOfPackagePriceReqeustHandler : IRequestHandler<GetListOfPackagePriceRequst, BaseResponse<Dictionary<PackageType, double>>>
{
  private readonly IEnumRepository<PackageType> _packageTypeRepository;
  private readonly IMapper _mapper;

  private readonly PaymentCalculatorService _paymentCalculatorService;
  public GetListOfPackagePriceReqeustHandler(IMapper mapper, PaymentCalculatorService paymentCalculatorService, IEnumRepository<PackageType> packageTypeRepository)
  {
    _mapper = mapper;
    _packageTypeRepository = packageTypeRepository;
    _paymentCalculatorService = paymentCalculatorService;

  }
  public Task<BaseResponse<Dictionary<PackageType, double>>> Handle(GetListOfPackagePriceRequst request, CancellationToken cancellationToken)
  {
    var response = new BaseResponse<Dictionary<PackageType, double>>();
    var prices = new Dictionary<PackageType, double>();

    foreach (PackageType packageType in Enum.GetValues(typeof(PackageType)))
    {
      var price = _paymentCalculatorService.CalculatePackagePrice(request.GetListOfPackagePriceReqeustDto.OneTimePrice, packageType);
      prices.Add(packageType, price);
    }

    response.Value = prices;
    response.IsSuccess = true;
    response.Message = "Package prices are successfully calculated";

    return Task.FromResult(response);

  }
}
