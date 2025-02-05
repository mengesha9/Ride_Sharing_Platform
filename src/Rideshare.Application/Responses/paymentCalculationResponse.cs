using Rideshare.Application.Responses;
using Rideshare.Domain.Common;

public class PaymentCalculationResponse : BaseCommandResponse<double>
{
  new public Dictionary<VehicleType, double> Value { get; set; } = default!;

  public static PaymentCalculationResponse Success(Dictionary<VehicleType, double> Vehicleprice, string message)
  {
    return new PaymentCalculationResponse
    {
      Succeeded = true,
      Value = Vehicleprice,
      Message = message
    };
  }

  public static PaymentCalculationResponse Failure(List<string> errors, string message)
  {
    return new PaymentCalculationResponse
    {
      Succeeded = false,
      Message = message,
      Errors = errors
    };
  }

}
