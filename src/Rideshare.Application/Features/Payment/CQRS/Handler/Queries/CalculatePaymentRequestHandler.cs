using MediatR;
using Rideshare.Application.Features.Payment.CQRS.Requests.Queries;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Payment.CQRS.Handlers;

public class CalculatePaymentRequestHandler : IRequestHandler<CalculatePaymentRequest, PaymentCalculationResponse>
{
  private readonly Dictionary<VehicleType, double> _vehicleTypePayment = new();
  private readonly Dictionary<PackageType, double> _packageTypeDiscount = new();
  private readonly Dictionary<PackageType, int> _packageNumDay = new();

  public CalculatePaymentRequestHandler()
  {
    _vehicleTypePayment.Add(VehicleType.Classic, 20);
    _vehicleTypePayment.Add(VehicleType.Economy, 10);
    _vehicleTypePayment.Add(VehicleType.Lada, 5);
    _vehicleTypePayment.Add(VehicleType.Minibus, 4);
    _vehicleTypePayment.Add(VehicleType.Minivan, 4);

    _packageTypeDiscount.Add(PackageType.Monthly, 0.05);
    _packageTypeDiscount.Add(PackageType.Weekly, 0.02);
    _packageTypeDiscount.Add(PackageType.Onetime, 0);
    _packageTypeDiscount.Add(PackageType.Quarterly, 0.1);

    _packageNumDay.Add(PackageType.Monthly, 30);
    _packageNumDay.Add(PackageType.Weekly, 7);
    _packageNumDay.Add(PackageType.Onetime,  1);
    _packageNumDay.Add(PackageType.Quarterly, 90);
  }

  public Task<PaymentCalculationResponse> Handle(CalculatePaymentRequest request, CancellationToken cancellationToken)
  {
    if (request == null)
    {
      return Task.FromResult(PaymentCalculationResponse.Failure(new List<string> { }, "No value given."));
    }

    double distance = request.calculatePaymentDto.distance;
    Dictionary<VehicleType, double> payments = new();
    foreach (VehicleType vehicleType in Enum.GetValues(typeof(VehicleType)))
    {
      var package=request.calculatePaymentDto.packageType;
      double price=distance * _vehicleTypePayment[vehicleType] * _packageNumDay[package];
      var discount=price*_packageTypeDiscount[package];
      Double payment =price-discount;
      payments.Add(vehicleType,payment);
    }

    return Task.FromResult(PaymentCalculationResponse.Success(payments, "Payment calculated successfully."));
  }
}
