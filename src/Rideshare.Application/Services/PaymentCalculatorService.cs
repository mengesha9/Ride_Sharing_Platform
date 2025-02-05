using Microsoft.Extensions.Options;
using Rideshare.Domain.Common;
using System;

namespace Rideshare.Application.Services
{
  public class PaymentCalculatorService
  {
    private readonly VehiclePricesOptions _vehiclePricesOptions;

    public PaymentCalculatorService(IOptions<VehiclePricesOptions> vehiclePricesOptions)
    {
      _vehiclePricesOptions = vehiclePricesOptions.Value;
    }

    public double CalculateVehiclePrice(double distance, VehicleType vehicleType)
    {
      int pricePerKm = vehicleType switch
      {
        VehicleType.Economy => _vehiclePricesOptions.Economy,
        VehicleType.Classic => _vehiclePricesOptions.Classic,
        VehicleType.Minivan => _vehiclePricesOptions.Minivan,
        VehicleType.Minibus => _vehiclePricesOptions.Minibus,
        VehicleType.Lada => _vehiclePricesOptions.Lada,
        _ => throw new ArgumentException("Invalid vehicle type")
      };

      return distance * pricePerKm;
    }
    public double CalculatePackagePrice(double OneTimePrice, PackageType packageType)
    {
      double packagePrice = packageType switch
      {
        PackageType.Monthly => (OneTimePrice * 20) * 0.8,
        PackageType.Onetime => OneTimePrice,
        PackageType.Quarterly => OneTimePrice * 60 * 0.7,
        PackageType.Weekly => OneTimePrice * 5 * 0.9,
        _ => throw new ArgumentException("Invalid package type")
      };

      return packagePrice;
    }
  }
}