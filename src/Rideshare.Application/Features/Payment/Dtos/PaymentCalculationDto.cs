
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Payment.Dtos;

public class CalculatePaymentDto
{
    public PackageType packageType {set; get;}
    public double distance {get; set;}

}