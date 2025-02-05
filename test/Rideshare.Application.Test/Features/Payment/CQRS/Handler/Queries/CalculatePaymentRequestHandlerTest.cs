using Rideshare.Application.Features.Payment.CQRS.Handlers;
using Rideshare.Application.Features.Payment.CQRS.Requests.Queries;
using Rideshare.Application.Features.Payment.Dtos;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Test.Features.Payment.CQRS.Handler.Queries;

public class CalculatePaymentRequestHandlerTest
{
  private readonly Dictionary<VehicleType, Double> _vehicleTypePayment = new();
  private readonly Dictionary<PackageType, Double> _packageTypeDiscount = new();
  private readonly Dictionary<PackageType, Double> _packageNumDay = new();

  public CalculatePaymentRequestHandlerTest()
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

  [Fact]
  public async Task CalculatePaymentRequestHandler_Success()
  {
    // Arrange
    var handler = new CalculatePaymentRequestHandler();
    var request = new CalculatePaymentRequest
    {
      calculatePaymentDto = new CalculatePaymentDto { distance = 10, packageType = PackageType.Monthly }
    };

    // Act
    var response = await handler.Handle(request, CancellationToken.None);

    // Assert
    Assert.NotNull(response);
    Assert.True(response.Succeeded);
  }

  [Fact]
  public async Task CalculatePaymentRequestHandler_Success_Distance()
  {
    // Arrange
    var handler = new CalculatePaymentRequestHandler();

    // Act
    var response1 = await handler.Handle(
      new CalculatePaymentRequest
      {
        calculatePaymentDto = new CalculatePaymentDto { distance = 10, packageType = PackageType.Monthly }
      },
      CancellationToken.None
    );

    var response2 = await handler.Handle(
      new CalculatePaymentRequest
      {
        calculatePaymentDto = new CalculatePaymentDto { distance = 100, packageType = PackageType.Monthly }
      },
      CancellationToken.None
    );

    // Assert
    Assert.NotNull(response1);
    Assert.NotNull(response2);
    Assert.True(response1.Succeeded);
    Assert.True(response2.Succeeded);
    Assert.True(response1.Value[VehicleType.Economy] < response2.Value[VehicleType.Economy]);
  }

  [Fact]
  public async Task CalculatePaymentRequestHandler_Success_PackageType()
  {
    // Arrange
    var handler = new CalculatePaymentRequestHandler();

    // Act
    var response1 = await handler.Handle(
      new CalculatePaymentRequest
      {
        calculatePaymentDto = new CalculatePaymentDto { distance = 10, packageType = PackageType.Weekly }
      },
      CancellationToken.None
    );

    var response2 = await handler.Handle(
      new CalculatePaymentRequest
      {
        calculatePaymentDto = new CalculatePaymentDto { distance = 10, packageType = PackageType.Monthly }
      },
      CancellationToken.None
    );

    // Assert
    Assert.NotNull(response1);
    Assert.NotNull(response2);
    Assert.True(response1.Succeeded);
    Assert.True(response2.Succeeded);
    Assert.True(response1.Value[VehicleType.Economy] < response2.Value[VehicleType.Economy]);
  }

  [Fact]
  public async Task CalculatePaymentRequestHandler_Success_Calculation()
  {
    // Arrange
    var handler = new CalculatePaymentRequestHandler();
    var distance = 10;
    var packageType = PackageType.Weekly;
    
    var correctPaymentForEconomy = distance * _vehicleTypePayment[VehicleType.Economy] *_packageNumDay[packageType];
    var discount=correctPaymentForEconomy*_packageTypeDiscount[packageType];
    correctPaymentForEconomy-=discount;

    // Act
    var response1 = await handler.Handle(
      new CalculatePaymentRequest
      {
        calculatePaymentDto = new CalculatePaymentDto { distance = 10, packageType = PackageType.Weekly }
      },
      CancellationToken.None
    );

    // Assert
    Assert.NotNull(response1);
    Assert.True(response1.Succeeded);
    Assert.Equal(correctPaymentForEconomy, response1.Value[VehicleType.Economy]);
  }
}
